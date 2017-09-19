using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountBook.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "MVC project - AccountBook.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "MVC project.";

            return View();
        }
    }
}