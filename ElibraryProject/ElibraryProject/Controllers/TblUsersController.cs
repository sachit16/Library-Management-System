using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElibraryProject.Controllers
{
    public class TblUsersController : Controller
    {
        public LibraryEntities1 userDb = new LibraryEntities1();

     
        public ActionResult Index()
        {
            return View(userDb.tblUsers.ToList());
        }

      
        public ActionResult GetAll()
        {
            var userlist = userDb.tblUsers.ToList();
            return Json(new { data = userlist }, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = userDb.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

      
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,UserGender,UserDep,UserAdmNo,UserEmail,UserPass")] tblUser tblUser)
        {
            Session.Remove("emailExists");
            if (ModelState.IsValid)
            {

                if (userDb.tblUsers.Where(u => u.UserEmail == tblUser.UserEmail).Count() > 0)
                {
                    Session["emailExists"] = "The Email address is already exists.";
                    return View(tblUser);
                }
                else
                {
                    Session["operationMsg"] = "User added successfully";
                    userDb.tblUsers.Add(tblUser);
                    userDb.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(tblUser);
        }

        public ActionResult OperationAlert()
        {
            Session.Remove("operationMsg");
            return RedirectToAction("Index");
        }

    
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = userDb.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,UserGender,UserDep,UserAdmNo,UserEmail,UserPass")] tblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                Session["operationMsg"] = "User updated successfully";
                userDb.Entry(tblUser).State = EntityState.Modified;
                userDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblUser);
        }

     
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = userDb.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblUser tblUser = userDb.tblUsers.Find(id);
            userDb.tblUsers.Remove(tblUser);
            userDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}