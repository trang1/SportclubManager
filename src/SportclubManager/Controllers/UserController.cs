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
                    if (cachedUser != null)
                    {
                        cachedUser.FirstName = user.FirstName;
                        cachedUser.LastName = user.LastName;

                        if (!cachedUser.IsCoach)
                            cachedUser.UserLogin = user.UserLogin;

                        if (cachedUser.UserID == cachedUser.CurrentUserId)
                            cachedUser.UserPassword = user.UserPassword;

                        if (!cachedUser.IsCoach)
                            cachedUser.Role = Db.Roles.FirstOrDefault(r => r.RoleID == int.Parse(user.SelectedRoleValue));
                    }
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