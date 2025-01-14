﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElibraryProject.Controllers
{
    
        public class AdminTransactionController : Controller
        {
            private LibraryEntities1 transDb = new LibraryEntities1();


        
            public ActionResult Requests()
            {
                return View(transDb.tblTransactions.ToList());
            }

            public ActionResult GetAllRequests()
            {
                var transactionList = transDb.tblTransactions.Where(r => r.TranStatus == "Requested").ToList();
                return Json(new { data = transactionList }, JsonRequestBehavior.AllowGet);
            }

            public ActionResult AcceptRequest(int? tranId)
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
                transaction.TranStatus = "Accepted";
                // transaction.TranDate = DateTime.Now.ToShortDateString();
                transDb.SaveChanges();
                return View("Requests");
              

            } 
            
            public ActionResult RejectRequest(int? tranId)
            {
                /*try
                {*/
                if (tranId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tblTransaction transaction = transDb.tblTransactions.FirstOrDefault(t => t.TranId == tranId);
                if (transaction == null)
                {
                    return HttpNotFound();
                }
                transaction.TranStatus = "Rejected";
                //transaction.TranDate = Date.Now.ToShortDateString();
                tblBook book = transDb.tblBooks.FirstOrDefault(b => b.BookId == transaction.BookId);
                book.BookCopies = book.BookCopies + 1;
                transDb.SaveChanges();
                transDb.SaveChanges();
                return View("Requests");
              
            }
         
            public ActionResult Accepted()
            {
                return View(transDb.tblTransactions.ToList());
            }
       
            public ActionResult GetAllAccepted()
            {
                var transactionList = transDb.tblTransactions.Where(r => r.TranStatus == "Accepted").ToList();
                return Json(new { data = transactionList }, JsonRequestBehavior.AllowGet);
            }
        
            public ActionResult Return()
            {
                return View(transDb.tblTransactions.ToList());
            }
         
            public ActionResult GetAllReturn()
            {
                var transactionList = transDb.tblTransactions.Where(r => r.TranStatus == "Returned").ToList();
                return Json(new { data = transactionList }, JsonRequestBehavior.AllowGet);
            }
          
            public ActionResult AcceptReturn(int? tranId)
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
                return View("Return");
              
            }
          
            public ActionResult AdminHome()
            {
                return View();
            }
           
            public ActionResult AdminAbout()
            {
                return View();
            }
         
            public ActionResult AdminContact()
            {
                return View();
            }
        }
    


}