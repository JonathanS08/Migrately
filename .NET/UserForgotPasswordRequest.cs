using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class UserForgotPasswordRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255), MinLength(2)]
        public string Email { get; set; }
    }
}
