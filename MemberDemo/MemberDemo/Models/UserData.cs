using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberDemo.Models
{
    public class UserData
    {
        public string UserName { get; set; }
        public string FullName
        {
            get { return this.FirstName+" "+this.LastName; }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLogin { get; set; }
    }
}