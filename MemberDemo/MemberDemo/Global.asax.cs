using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MemberDemo
{
    // 注意: 如需啟用 IIS6 或 IIS7 傳統模式的說明，
    // 請造訪 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            bool hasUser = HttpContext.Current.User != null;
            bool isAuthenticated = hasUser && HttpContext.Current.User.Identity.IsAuthenticated;
            bool isIdentity = isAuthenticated && HttpContext.Current.User.Identity is System.Web.Security.FormsIdentity;

            if (isIdentity)
            {
                // 取得表單認證目前這位使用者的身份
                var id = (System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity;
                // 取得 FormsAuthenticationTicket 物件
                var ticket = id.Ticket;
                // 取得 UserData 欄位資料 (這裡儲存的是角色) ，如果有多個角色可以用逗號分隔
                string[] roles = ticket.UserData.Split(',');
                // 賦予該使用者新的身份 (含角色資訊)
                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //如果Session消失，強制登出

            if (HttpContext.Current.Session != null)
            {
                if (Session["UserData"] == null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Response.Redirect("~/");
                    }
                }
            }
        }
    }
}