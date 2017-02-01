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
            if (userId == -1)
                user = new User() {UserID = -1};
            return View(user ?? CurrentUser);
        }

        [HttpPost]
        public ActionResult Save(User user)
        {
            if (user != null)
            {
                if (user.UserID == -1)
                {
                    Db.Users.InsertOnSubmit(user);
                }
                else
                {
                    var cachedUser = Db.Users.First(u => u.UserID == user.UserID);
                    cachedUser.FirstName = user.FirstName;
                    cachedUser.LastName = user.LastName;

                    if (!CurrentUser.IsCoach)
                        cachedUser.UserLogin = user.UserLogin;

                    if (cachedUser.UserID == CurrentUser.UserID)
                        cachedUser.UserPassword = user.UserPassword;

                    if (!CurrentUser.IsCoach)
                        cachedUser.RoleID = user.RoleID;
                }
                Db.SubmitChanges();
            }
            else
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int userid)
        {
            var cachedUser = Db.Users.First(u => u.UserID == userid);
            if (cachedUser != null)
            {
                Db.Users.DeleteOnSubmit(cachedUser);
                Db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}