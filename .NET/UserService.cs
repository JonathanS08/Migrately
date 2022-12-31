using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Sabio.Models.Requests.Messages;
using Sabio.Data;
using System.Collections.Generic;
using Sabio.Models.Requests.Users;
using Sabio.Models.Domain.Users;
using System;
using Sabio.Models.Enums;
using Stripe;
using System.Xml.Linq;

namespace Sabio.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;

        public UserService(IAuthenticationService<int> authSerice, IDataProvider dataProvider)
        {
            _authenticationService = authSerice;
            _dataProvider = dataProvider;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(email, password);

            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public async Task<bool> LogInTest(string email, string password, int id, string[] roles = null)
        {
            bool isSuccessful = false;
            var testRoles = new[] { "User", "Super", "Content Manager" };

            var allRoles = roles == null ? testRoles : testRoles.Concat(roles);

            IUserAuthData response = new UserBase
            {
                Id = id
                ,
                Name = email
                ,
                Roles = allRoles
                ,
                TenantId = "Migrately UId"
            };

            Claim fullName = new Claim("CustomClaim", "Sabio Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });

            return isSuccessful;
        }

        public int Create(UserAddRequest userModel)
        {



            int userId = 0;

            string procName = "[dbo].[Users_Insert]";

            string password = userModel.Password;

            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

            _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Password", hashedPassword);
                AddCommonParams(userModel, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out userId);
            });





            return userId;
        }

        /// <summary>
        /// Gets the Data call to get a give user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        private IUserAuthData Get(string email, string password)
        {

            string passwordFromDb = "";
            UserBase user = null;
            UserAuth userFromDB = null;
            string procName = "[dbo].[Users_Select_AuthDataV2]";


            _dataProvider.ExecuteCmd(procName,
                delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Email", email);
                }
                , delegate (IDataReader reader, short set)
                {
                    userFromDB = new UserAuth();

                    int startingIndex = 0;
                    userFromDB.Id = reader.GetSafeInt32(startingIndex++);
                    userFromDB.Email = reader.GetSafeString(startingIndex++);
                    passwordFromDb = reader.GetSafeString(startingIndex++);
                    userFromDB.StatusId = reader.GetSafeInt32(startingIndex++);
                    string roles = reader.GetSafeString(startingIndex++);

                    if (!string.IsNullOrEmpty(roles))
                    {
                        userFromDB.Roles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LookUp>>(roles);
                    };
                });
            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);

            if (isValidCredentials)
            {
                user = new UserBase();
                user.Name = userFromDB.Email;
                user.Id = userFromDB.Id;
                if (userFromDB.Roles != null)
                {
                    user.Roles = userFromDB.Roles.Select(x => x.Name).ToArray();
                }

                user.TenantId = "Migrately UId";
            }

            return user;
        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Mi", model.Mi);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);

        }

        public void AddToken(int userId, string token, int tokenType)
        {
            string procName = "[dbo].[UserTokens_Insert]";

            _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Token", token);
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@TokenType", tokenType);
            });
        }

        public ValidUser ConfirmEmail(string email, string token)
        {
            string procName = "dbo.Users_SelectId_ByEmailToken";
            ValidUser confirmUser = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Email", email);
                paramCollection.AddWithValue("@Token", token);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                confirmUser = new ValidUser();

                confirmUser.Id = reader.GetSafeInt32(startingIndex++);
                confirmUser.Email = reader.GetSafeString(startingIndex++);
                confirmUser.Token = reader.GetSafeString(startingIndex++);
            });
            return confirmUser;
        }

        public bool UpdateConfirmation(int id, bool isConfirmed)
        {
            bool result = false;
            string procName = "[dbo].[Users_Confirm]";
            int rowsUpdated = _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
                col.AddWithValue("@IsConfirmed", isConfirmed);
            });
            if (rowsUpdated > 0)
            {
                result = true;
            }
            return result;
        }

        public void DeleteToken(string token, int id)
        {
            string procName = "[dbo].[UserTokens_Delete_ByToken]";
            _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Token", token);
                col.AddWithValue("@Id", id);
            });
        }

        public User GetById(int id)
        {
            User user = null;

            string procName = "[dbo].[Users_SelectById]";
            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , delegate (IDataReader reader, short set)
            {
                user = new User();
                int startingIndex = 0;
                user = SingleUserMapper(reader, ref startingIndex);

            });
            return user;
        }

        private static User SingleUserMapper(IDataReader reader, ref int startingIndex)
        {
            User user = new User();
            user.Status = new LookUp();


            user.UserId = reader.GetSafeInt32(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.MiddleInitial = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.Status.Id = reader.GetSafeInt32(startingIndex++);
            user.Status.Name = reader.GetSafeString(startingIndex++);
            user.Role = reader.DeserializeObject<List<LookUp>>(startingIndex++);

            return user;
        }


        public void Update(UsersStatusUpdateRequest model)
        {
            string procName = "[dbo].[Users_UpdateStatus]";

            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);

                    col.AddWithValue("@StatusId", model.StatusId);
                },
                returnParameters: null);
        }

        public Paged<User> Pagination(int pageIndex, int pageSize)
        {
            Paged<User> pagedResult = null;

            List<User> result = null;

            string procName = "[dbo].[Users_SelectAllV2]";

            int totalCount = 0;

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    User users = SingleUserMapper(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (result == null)
                    {
                        result = new List<User>();
                    }

                    result.Add(users);
                });

            if (result != null)
            {
                pagedResult = new Paged<User>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

#nullable enable
        public Paged<User> SearchPagination(int pageIndex, int pageSize, string? query, int? role, int? status)
#nullable disable
        {
            Paged<User> pagedResult = null;

            List<User> result = null;

            string procName = "[dbo].[Users_Search]";

            int totalCount = 0;

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                    parameterCollection.AddWithValue("@RoleId", role);
                    parameterCollection.AddWithValue("@StatusId", status);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    User users = SingleUserMapper(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (result == null)
                    {
                        result = new List<User>();
                    }

                    result.Add(users);
                });

            if (result != null)
            {
                pagedResult = new Paged<User>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }
#nullable disable

        public Paged<User> SearchStatus(int pageIndex, int pageSize, string query)
        {
            Paged<User> pagedResult = null;

            List<User> result = null;

            string procName = "[dbo].[Users_SearchStatus]";

            int totalCount = 0;

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    User users = SingleUserMapper(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (result == null)
                    {
                        result = new List<User>();
                    }

                    result.Add(users);
                });

            if (result != null)
            {
                pagedResult = new Paged<User>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<User> SearchRole(int pageIndex, int pageSize, string query)
        {
            Paged<User> pagedResult = null;

            List<User> result = null;

            string procName = "[dbo].[Users_SearchRoles]";

            int totalCount = 0;

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    User users = SingleUserMapper(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (result == null)
                    {
                        result = new List<User>();
                    }

                    result.Add(users);
                });

            if (result != null)
            {
                pagedResult = new Paged<User>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public UserAuth GetUserAuth(UserForgotPasswordRequest model)
        {

            UserAuth userFromDB = null;
            string procName = "[dbo].[Users_Select_AuthData]";
            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Email", model.Email);
            }
            , delegate (IDataReader reader, short set)
            {
                userFromDB = new UserAuth();
                int startingIndex = 0;
                userFromDB = SingleUserAuthMapper(reader, startingIndex);

            }
            );
            return userFromDB;
        }

        public void UpdateUserData(UserUpdateRequest model)
        {
            string procName = "[dbo].[Users_Data_Update]";

            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: (SqlParameterCollection col) =>
            {
                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                col.AddWithValue("@Id", model.Id);
            }, returnParameters: null);
        }

        public bool UpdatePassword(string password, int id)
        {
            bool result = false;
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

            string procName = "dbo.Users_Update_Password";
            int rowsUpdated = _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
                col.AddWithValue("@Password", hashedPassword);
            });
            if (rowsUpdated > 0)
            {
                result = true;
            }
            return result;
        }

        private static UserAuth SingleUserAuthMapper(IDataReader reader, int startingIndex)
        {
            UserAuth userFromDB = new UserAuth();

            userFromDB.Id = reader.GetSafeInt32(startingIndex++);
            userFromDB.Email = reader.GetSafeString(startingIndex++);
            userFromDB.Password = reader.GetSafeString(startingIndex++);
            string roles = reader.GetSafeString(startingIndex++);

            if (!string.IsNullOrEmpty(roles))
            {
                userFromDB.Roles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LookUp>>(roles);
            };
            return userFromDB;
        }

        public int AddRoles(UserRolesAddRequest model)
        {
            int id = 0;

            DataTable dataTable = null;

            if(model.RoleId != null)
            {
                dataTable = MapModelsToTable(model.RoleId);
            }

            string procName = "[dbo].[UserRoles_Insert]";

            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@batchRoleIds", dataTable);
                    col.AddWithValue("@UserId", model.UserId);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@UserId"].Value;

                    int.TryParse(oId.ToString(), out id);
                });
                return id;
        }

        private DataTable MapModelsToTable(List<int> RoleId)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));

            foreach (int Id in RoleId)
            {
                DataRow dr = dt.NewRow();
                int startingIndex = 0;

                dr.SetField(startingIndex++, Id);

                dt.Rows.Add(dr);
            }

            return dt;
        }

    }
}