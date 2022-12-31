using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255), MinLength(2)]
        public string Email { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please enter a valid password" +
            ", Has minimum 8 characters in length. At least one uppercase English letter. At least one lowercase English letter.  " +
            "At least one digit. At least one special character.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
