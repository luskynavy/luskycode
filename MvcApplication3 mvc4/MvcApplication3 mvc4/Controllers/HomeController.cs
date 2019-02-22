using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcApplication3_mvc4.Models;

namespace MvcApplication3_mvc4.Controllers
{
    public class HomeController : Controller
    {
        Database1Entities db = new Database1Entities();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //return View();
            return RedirectToAction("ListBooks");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult ListBooks()
        {            
            return View(db.Book.ToList());
        }

        public ActionResult ListBooks2()
        {
            return View(db.Book.ToList());
        }

        //Create
        public ActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBook(Book book)
        {
            db.Book.AddObject(book);
            db.SaveChanges();

            return RedirectToAction("ListBooks");
        }

        //Edit
        public ActionResult EditBook(int id)
        {
            Book bookToEdit = db.Book.FirstOrDefault(b => b.id == id);

            return View(bookToEdit);
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            Book bookToEdit = db.Book.FirstOrDefault(b => b.id == book.id);
            UpdateModel(bookToEdit);     
            db.SaveChanges();

            return RedirectToAction("ListBooks");
        }

        //Delete
        public ActionResult DeleteBook(int id)
        {
            Book bookToDelete = db.Book.FirstOrDefault(b => b.id == id);

            return View(bookToDelete);
        }

        [HttpPost]
        public ActionResult DeleteBook(Book book)
        {
            Book  bookToDelete = db.Book.FirstOrDefault(b => b.id == book.id);
            db.Book.DeleteObject(bookToDelete);
            db.SaveChanges();

            return RedirectToAction("ListBooks");
        }

        //Details
        public ActionResult DetailsBook(int id)
        {
            Book bookToView = db.Book.FirstOrDefault(b => b.id == id);

            return View(bookToView);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
