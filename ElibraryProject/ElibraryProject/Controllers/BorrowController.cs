using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElibraryProject.Controllers
{
    public class BorrowController : Controller
    {
        static int userId;        
        static string userName;     

        private LibraryEntities1 userDb = new LibraryEntities1();
      


      
        public ActionResult Index(int? userId, string userName)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = userDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            BorrowController.userId = (int)userId;
            BorrowController.userName = userName;
            return View(userDb.tblBooks.ToList());
        }

        public ActionResult UserHome()
        {
            return View();
        }

      
        public ActionResult UserAbout()
        {
            return View();
        }

      
        public ActionResult UserContact()
        {
            return View();
        }

      
        public ActionResult MenuBorrow()
        {
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
        }

      
        public ActionResult MenuRequested()
        {
            return RedirectToAction("Requested", "UserTransaction", new { userId = userId });
        }

      
        public ActionResult MenuReceived()
        {
            Session.Remove("receivedBadge");
            return RedirectToAction("Received", "UserTransaction", new { userId = userId });
        }

     
        public ActionResult MenuRejected()
        {
            Session.Remove("rejectedBadge");
            return RedirectToAction("Rejected", "UserTransaction", new { userId = userId });
        }

     
        public ActionResult Borrow(int? bookId)
        {
           
            if (userDb.tblTransactions.Where(t => t.UserId == userId).Count() < 6)
            {
                if (bookId != null)
                {
                    tblBook book = userDb.tblBooks.FirstOrDefault(b => b.BookId == bookId);
                    if (book == null)
                    {
                        return HttpNotFound();
                    }
                    if (book.BookCopies > 0)
                    {
                        book.BookCopies = book.BookCopies - 1;
                        tblTransaction trans = new tblTransaction()
                        {
                            BookId = book.BookId,
                            BookTitle = book.BookTitle,
                            BookISBN = book.BookISBN,
                          //TranDate = book.DateAdded,
                            TranStatus = "Requested",
                            UserId = userId,
                            UserName = userName,
                        };
                        userDb.SaveChanges();
                        userDb.tblTransactions.Add(trans);
                        userDb .SaveChanges();
                        Session["requestMsg"] = "Requested successfully";
                    }
                    else
                    {
                        Session["requestMsg"] = "Sorry you cant take, Book copy is zero";
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                Session["requestMsg"] = "Sorry you cant take more than six books";
            }
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
          
        }

      
        public ActionResult RequestAlert()
        {
            Session.Remove("requestMsg");
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
        }
    }
}