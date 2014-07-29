using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MemberDemo.Models
{
    public class Member
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(256)]
        public string Password { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class MemberContext : DbContext
    {
        public DbSet<Member> Members { get; set; }

        //public MemberContext()
        //    : base("name=MemberDemoConnection")
        //{ 
        
        //}
    }
}