using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElibraryProject.Controllers
{
    public class AdminController : Controller
    {
        private LibraryEntities1 adminDb = new LibraryEntities1();

   
        [HttpGet]
        [HandleError]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HandleError]
        public ActionResult Login(tblAdmin admin)
        {
            var adm = adminDb.tblAdmins.SingleOrDefault(a => a.AdimEmail == admin.AdimEmail && a.AdimPass == admin.AdimPass);
            if (adm != null)
            {
                int id = adm.AdminId;
                Session["adminId"] = adm.AdminId;
                return RedirectToAction("Index", "TblBooks", new { id = id });
            }
            else if (admin.AdimEmail == null && admin.AdimPass == null)
            {
                return View();
            }
            ViewBag.Message = "User name and password are not matching";
            return View();
        }
        [HandleError]
        public ActionResult Logout()
        {
            Session.Remove("adminId");
            return RedirectToAction("Home", "Main");
        }
    }
}