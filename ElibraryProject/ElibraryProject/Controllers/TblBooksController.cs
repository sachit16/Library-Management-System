using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElibraryProject.Controllers
{
    public class TblBooksController : Controller
    {
        private LibraryEntities1 bookDb = new LibraryEntities1();

      
        public ActionResult Index()
        {
            return View(bookDb.tblBooks.ToList());
        }
     
        public ActionResult GetAll()
        {
            var booklist = bookDb.tblBooks.ToList();
            return Json(new { data = booklist }, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

     
        public ActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,BookTitle,BookCategory,BookAuthor,BookCopies,BookPub,BookPubName,BookISBN,Copyright,DateAdded,Status")] tblBook tblBook)
        {
            if (ModelState.IsValid)
            {
                Session["operationMsg"] = "Book added successfully";
                bookDb.tblBooks.Add(tblBook);
                bookDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblBook);
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
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,BookTitle,BookCategory,BookAuthor,BookCopies,BookPub,BookPubName,BookISBN,Copyright,DateAdded,Status")] tblBook tblBook)
        {
            if (ModelState.IsValid)
            {
                Session["operationMsg"] = "Book updated successfully";
                bookDb.Entry(tblBook).State = EntityState.Modified;
                bookDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblBook);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblBook tblBook = bookDb.tblBooks.Find(id);
            bookDb.tblBooks.Remove(tblBook);
            bookDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bookDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}