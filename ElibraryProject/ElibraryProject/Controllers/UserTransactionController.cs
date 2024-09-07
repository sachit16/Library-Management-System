using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElibraryProject.Controllers
{
    public class UserTransactionController : Controller
    {
        static int userId;     

        LibraryEntities1 transDb = new LibraryEntities1();
        

        public ActionResult Requested(int? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = transDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserTransactionController.userId = (int)userId;
            var requestList = transDb.tblTransactions.Where(s => s.TranStatus == "Requested" && s.UserId == userId);
            if (requestList.Count() == 0)
            {
                Session["requestMessage"] = "Your Requested list is empty, Go to Borrow section for request a book.";
            }
            else
            {
                Session.Remove("requestMessage");
            }
            return View(requestList.ToList());
        }


        public ActionResult DeleteRequest(int? tranId)
        {

            if (tranId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            tblBook book = transDb.tblBooks.FirstOrDefault(b => b.BookId == transaction.BookId);
            book.BookCopies = book.BookCopies + 1;
            transDb.SaveChanges();
            transDb.tblTransactions.Remove(transaction);
            transDb.SaveChanges();
            return RedirectToAction("Requested", "UserTransaction", new { userId = userId });
        }
        public ActionResult Rejected(int? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = transDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserTransactionController.userId = (int)userId;
            var rejectedList = transDb.tblTransactions.Where(s => s.TranStatus == "Rejected" && s.UserId == userId);
            if (rejectedList.Count() == 0)
            {
                Session["rejectMessage"] = "Your Rejected list is empty, Wait for the admin to take action.";
            }
            else
            {
                Session.Remove("rejectMessage");
            }
            return View(rejectedList.ToList());
        }


        public ActionResult RerequestRejected(int? tranId)
        {
          
            if (tranId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            transaction.TranStatus = "Requested";
          //  transaction.TranDate = .Now.ToShortDateString();
            tblBook book = transDb.tblBooks.FirstOrDefault(b => b.BookId == transaction.BookId);
            book.BookCopies = book.BookCopies - 1;
            transDb.SaveChanges();
            transDb.SaveChanges();
            return RedirectToAction("Rejected", "UserTransaction", new { userId = userId });
           
        }

        public ActionResult CancelRejected(int? tranId)
        {
          
            if (tranId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            tblBook book = transDb.tblBooks.FirstOrDefault(b => b.BookId == transaction.BookId);
            book.BookCopies = book.BookCopies + 1;
            transDb .SaveChanges();
            transDb.tblTransactions.Remove(transaction);
            transDb.SaveChanges();
            return RedirectToAction("Rejected", "UserTransaction", new { userId = userId });
          
        }

      
        public ActionResult Received(int? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = transDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserTransactionController.userId = (int)userId;
            var receivedList = transDb.tblTransactions.Where(s => s.TranStatus == "Accepted" && s.UserId == userId);
            if (receivedList.Count() == 0)
            {
                Session["receiveMessage"] = "Your Received list is empty, Wait for the admin to take action.";
            }
            else
            {
                Session.Remove("receiveMessage");
            }
            return View(receivedList.ToList());
        }

     
        public ActionResult ReturnReceived(int? tranId)
        {
           
            if (tranId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
            if (transaction == null)
            {
                return HttpNotFound();
            }
         
            transaction.TranStatus = "Returned";
            transDb.SaveChanges();
            return RedirectToAction("Received", "UserTransaction", new { userId = userId });
         
        }
    }
}