using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Auth;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            if (UserProvider.CurrentUser.IsCoach)
                return RedirectToNotFoundPage;

            var users = Db.Users.OrderBy(u => u.UserID).ToList();
            return View(users);
        }

        public ActionResult Info(int? userId)
        {
            var user = Db.Users.FirstOrDefault(u => u.UserID == userId);
            if (userId == -1)
                user = new User() {UserID = -1};
            return View(new UserModel(user ?? UserProvider.CurrentUser));
        }

        [HttpPost]
        public ActionResult Save(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = userModel.GetUser();
                if (user.UserID == -1)
                {
                    Db.Users.InsertOnSubmit(user);
                }
                else
                {
                    var cachedUser = Db.Users.First(u => u.UserID == user.UserID);
                    cachedUser.FirstName = user.FirstName;
                    cachedUser.LastName = user.LastName;

                    if (!UserProvider.CurrentUser.IsCoach)
                        cachedUser.UserLogin = user.UserLogin;

                    if (cachedUser.UserID == UserProvider.CurrentUser.UserID)
                        cachedUser.UserPassword = user.UserPassword;

                    if (!UserProvider.CurrentUser.IsCoach)
                        cachedUser.RoleID = user.RoleID;
                }
                Db.SubmitChanges();

                //delete all related group references
                var groupsForDelete = Db.Groups.Where(g => g.CoachID == user.UserID).ToList();
                groupsForDelete.ForEach(g=>g.CoachID = null);
                //set new related groups
                if (user.SelectedGroups != null)
                {
                    foreach (var selectedGroup in user.SelectedGroups)
                    {
                        var group = Db.Groups.First(g => selectedGroup == g.GroupID.ToString());
                        group.CoachID = user.UserID;
                    }
                }
                Db.Groups.Context.SubmitChanges();
            }
            else
            {
                return View("Info", userModel);
            }
            return UserProvider.CurrentUser.IsCoach ? RedirectToAction("Home", "Home") : RedirectToAction("Index");
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