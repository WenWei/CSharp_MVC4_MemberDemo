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
        /// 登入
        /// </summary>
        /// <param name="m"></param>
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

                    string userData = "";//可放使用者自訂的內容
                    //寫cookie
                    //使用 Cookie 名稱、版本、到期日、核發日期、永續性和使用者特定的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。 此 Cookie 路徑設定為在應用程式的組態檔中建立的預設值。
                    //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                    var ticket = new System.Web.Security.FormsAuthenticationTicket(1,
                      account.UserName,//使用者ID
                      DateTime.Now,//核發日期
                      DateTime.Now.AddDays(5),//到期日期 60分鐘 
                      m.IsRemember,//永續性
                      userData,//使用者定義的資料
                      System.Web.Security.FormsAuthentication.FormsCookiePath);

                    string encTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encTicket));

                    //設定使用者登入資訊
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
                    //登入失敗
                    ModelState.AddModelError("", "登入失敗，帳號不存在或密碼錯誤");
                    return View(m);
                }
            }
        }

        /// <summary>
        /// 註冊新帳號
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
                    
                    //帳號已存在
                    if (account != null)
                    {
                        throw new ArgumentException("帳號已存在");
                        //return Json(new RContent
                        //{
                        //    err = 1,
                        //    msg = "Duplicate username."
                        //});
                    }

                    db.Members.Add(new Member
                    {
                        UserName = m.UserName,
                        //不存明碼，暫用 GetHashCode
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
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();

            //清除目前使用者的所有的 session
            Session.RemoveAll();

            //建立一個同名的 Cookie 來覆蓋原本的 Cookie
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
