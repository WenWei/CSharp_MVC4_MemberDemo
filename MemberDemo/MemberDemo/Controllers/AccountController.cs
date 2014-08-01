using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MemberDemo.Models;

namespace MemberDemo.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        /// <summary>
        /// Login & Signup page
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //登入狀態重導至 /Manage/Index
            if (WebModels.UserSession.UserData != null)
            {
                return RedirectToAction("Index", "Manage");
            }

            return View();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="m">LoginModel of account information</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginModel m)
        {
            using (var dbx = new Models.MemberContext())
            {
                var account = dbx.Members.Find(m.UserName);
                if (account != null && account.Password == m.Password.GetHashCode().ToString())
                {
                    //登入成功
                    //System.Web.Security.FormsAuthentication.SetAuthCookie(u.Email, model.RememberMe);

                    account.LastLoginDate = DateTime.Now;
                    dbx.SaveChanges();

                    string userData = "";           // Custom account information
                    //Create cookie
                    //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                    var ticket = new System.Web.Security.FormsAuthenticationTicket(1,
                      account.UserName,             // user account
                      DateTime.Now,                 // Issue date
                      DateTime.Now.AddMinutes(60),  // Expire time 60 minutes 
                      m.IsRemember,                 // Persistent
                      userData,                     // Custom account information
                      System.Web.Security.FormsAuthentication.FormsCookiePath);

                    string encTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encTicket));

                    // Set user login information into session
                    WebModels.UserSession.UserData = new UserData { 
                        UserName=account.UserName,
                        FirstName=account.FirstName,
                        LastName=account.LastName,
                        LastLogin=account.LastLoginDate.Value
                    };

                    return RedirectToRoute("Default", new { controller = "Manage" });
                }
                else
                {
                    // Login fail
                    ModelState.AddModelError("", "Login failed, the account does not exist or the password is wrong");
                    return View(m);
                }
            }
        }

        /// <summary>
        /// Signup new user account
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Signup(Models.SignupModel m)
        {
            try
            {
                if (m.UserName.Trim().Length < 3) { throw new FormatException("帳號長度不符，最小長度3"); }
                if (m.UserName.Trim().Length > 50) { throw new FormatException("帳號長度不符，最大長度50"); }
                
                using (var db = new Models.MemberContext())
                {
                    var account = db.Members.Find(m.UserName);
                    
                    // Account exists
                    if (account != null)
                    {
                        throw new ArgumentException("Account exists.");
                        //return Json(new RContent
                        //{
                        //    err = 1,
                        //    msg = "Duplicate username."
                        //});
                    }

                    db.Members.Add(new Member
                    {
                        UserName = m.UserName,
                        //DB does not store the original password, temporary use GetHashCode
                        Password = m.Password.GetHashCode().ToString(),
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        CreatedOn = DateTime.Now
                    });
                    db.SaveChanges();

                    return Json(new RContent<string> { 
                        err=0,
                        msg="Success",
                        data=Url.Action("Index","Manage")
                    });
                }

            }
            catch (Exception ex)
            {
                // TODO: Log exception
                
                return Json(new
                {
                    err=1,
                    msg=ex.Message
                });
            }

            
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();

            //Clear current session
            Session.RemoveAll();

            // Create new cookie rewrite cookie. Ensure cookie is empty.
            HttpCookie cookieEmpty = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, "");
            cookieEmpty.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookieEmpty);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookieASPSession = new HttpCookie("ASP.NET_SessionId", "");
            cookieASPSession.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookieASPSession);

            return RedirectToAction("Login", "Account");
        }

    }
}
