using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            var users = Db.Users.OrderBy(u => u.UserID).ToList();
            return View(users);
        }

        public ActionResult EditUser()
        {
            return View(CurrentUser);
        }

        public ActionResult ShowProfile(int userId)
        {
            return View(userId);
        }
    }
}