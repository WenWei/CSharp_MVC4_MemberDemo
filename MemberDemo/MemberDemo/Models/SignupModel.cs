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
        [MinLength(3, ErrorMessage = "欄位字數不足，最小長度 3")]
        [MaxLength(50, ErrorMessage="欄位超過字數限制，最大長度 50")]
        public string UserName { get; set; }
        [Required]
        [MinLength(6, ErrorMessage="欄位字數不足，最小長度 6")]
        [MaxLength(256, ErrorMessage = "欄位超過字數限制，最大長度 256")]
        public string Password { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
    }
}