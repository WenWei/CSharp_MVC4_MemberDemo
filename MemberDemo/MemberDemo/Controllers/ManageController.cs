using MemberDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberDemo.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        static readonly MemberContext db = new MemberContext();
        const int PAGESIZE = 20;
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            ViewBag.TotalPages = Math.Ceiling((double)db.Members.Count() / PAGESIZE);
            return View();

        }

        /// <summary>
        /// Get members
        /// </summary>
        /// <param name="pi">Page number，zero base</param>
        /// <param name="ps">Page size</param>
        /// <returns>會員資料陣列</returns>
        public JsonResult GetMembers(int pi, int ps=20)
        {
            if (pi < 0) pi = 0;
            var result = db.Members
                .OrderBy(m => m.UserName)
                .Skip(ps * pi)
                .Take(ps)
                .ToArray()
                .Select(m => new { 
                    m.UserName,
                    FullName=m.FirstName+ " "+m.LastName,
                    m.CreatedOn,
                    m.LastLoginDate
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete member account.
        /// </summary>
        /// <param name="id">UserName</param>
        /// <returns>err=0 delete success or account not exists; err=1 has unknow exception.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteMember(string id)
        {
            try
            {
                var member = db.Members.Find(id);
                if (member != null)
                {
                    db.Members.Remove(member);
                    db.SaveChanges();
                }

                return Json(new RContent
                {
                    err = 0,
                    msg = "success"
                });
            }
            catch (Exception ex)
            {
                return Json(new RContent { 
                    err=1,
                    msg=ex.Message
                });
            }
        }

    }
}
