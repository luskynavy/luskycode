using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcApplication3_mvc4.Models;
using PagedList;

//i18n : https://afana.me/archive/2011/01/14/aspnet-mvc-internationalization.aspx/

//paging, sorting and filtering
//https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application

//Book id int identity not null primary key
//Book name nchar(20)
//Book note nchar(10)

//Boorrowed id int identity not null primary key
//Boorrowed bookId int not null foreign key de Book id
//Boorrowed date datetime2(7)

namespace MvcApplication3_mvc4.Controllers
{
    public class HomeController : Controller
    {
        private Database1Entities db = new Database1Entities();

        private static DayHoursModel _dh = new DayHoursModel();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //return View();
            return RedirectToAction("ListBooksSearch");
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

        //Fix for adding paging and using Html.DisplayNameFor
        //https://stackoverflow.com/questions/14929311/using-html-displaynamefor-with-pagedlist
        public ActionResult ListBooksPaged(string name, int? page)
        {
            ViewBag.name = name;

            IQueryable<Book> books = db.Book;
            if (!String.IsNullOrEmpty(name))
            {
                books = books.Where(b => b.name.Contains(name));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(books.OrderBy(b => b.id).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListBooksSearch(SearchBook form)
        {
            IQueryable<Book> books = db.Book;
            if (!String.IsNullOrEmpty(form.NameSearch))
            {
                books = books.Where(b => b.name.Contains(form.NameSearch));
            }
            //form.BooksList = books.ToList();

            int pageSize = 5;
            int pageNumber = form.Page <= 0 ? 1 : form.Page;
            form.BooksList = books.OrderBy(b => b.id).ToPagedList(pageNumber, pageSize);
            return View(form);
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
            Book bookToDelete = db.Book.FirstOrDefault(b => b.id == book.id);
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

        public ActionResult DayHoursModelView()
        {
            return View(_dh);
        }

        //Edit
        public ActionResult EditDayHoursModel()
        {
            return View(_dh);
        }

        [HttpPost]
        public ActionResult EditDayHoursModel(DayHoursModel dh)
        {
            _dh = dh;

            return RedirectToAction("ListBooks");
        }
    }
}