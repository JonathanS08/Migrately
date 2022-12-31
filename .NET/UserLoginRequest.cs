using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class UserLoginRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255, MinimumLength = 2)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 2)]
        public string Password { get; set; }
    }
}
