using MemberDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberDemo.WebModels
{
    public class UserSession
    {
        /// <summary>
        /// User login information
        /// </summary>
        public static UserData UserData
        {
            get { 
                var userData=HttpContext.Current.Session["UserData"];
                if (userData == null)
                {
                    return null;
                }
                else
                {
                    return (UserData)userData;
                }
            }
            set { HttpContext.Current.Session["UserData"] = value; }
        }
        
        
        
    }
}