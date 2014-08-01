using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberDemo.Models
{
    public class SignupModel
    {
        [Required]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "Invalid UserName")]
        [MinLength(3, ErrorMessage = "Your username is required to be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Your username cannot be longer than 50 characters")]
        public string UserName { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Your password is required to be at least 6 characters")]
        [MaxLength(256, ErrorMessage = "Your password cannot be longer than 256 characters")]
        public string Password { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
    }
}