using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var roles = Db.Roles.ToList();
            ViewBag.Message = "Roles in this application:";

            return View(roles);
        }
    }
}