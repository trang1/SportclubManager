using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using SportclubManager.Auth;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            if (CurrentUser != null)
            {
                switch (CurrentUser.Role.RoleName)
                {
                    case Roles.Administrator:
                        return RedirectToAction("Index", "Admin");

                    case Roles.Coach:
                        return RedirectToAction("Index", "Coach");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}