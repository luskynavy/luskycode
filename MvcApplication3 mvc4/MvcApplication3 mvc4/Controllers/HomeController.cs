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

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult ListBooks()
        {            
            return View(db.Books.ToList());
        }

        public ActionResult ListBooks2()
        {
            return View(db.Books.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
