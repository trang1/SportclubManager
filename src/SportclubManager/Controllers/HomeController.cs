using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public SportclubManagerDataClassesDataContext Db { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var roles = Db.Roles.ToList();
            ViewBag.Message = "Your application description page.";

            return View(roles);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}