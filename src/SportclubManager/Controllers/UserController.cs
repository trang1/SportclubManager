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
        
        public ActionResult Info(int? userId)
        {
            var user = Db.Users.FirstOrDefault(u => u.UserID == userId);
            if(userId == -1)
                user = new User() {UserID = -1};

            return View(user ?? CurrentUser);
        }

        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }
    }
}