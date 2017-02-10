using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Auth;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class GroupController : BaseController
    {
        // GET: Group
        public ActionResult Index()
        {
            if (UserProvider.CurrentUser == null)
                return RedirectToAction("Home", "Home");

            if (UserProvider.CurrentUser.IsCoach)
                return RedirectToNotFoundPage;

            var groups = Db.Groups.OrderBy(u => u.GroupID).ToList();
            return View(groups);
        }

        public ActionResult Info(int groupId)
        {
            if (UserProvider.CurrentUser == null)
                return RedirectToAction("Home", "Home");

            var group = Db.Groups.FirstOrDefault(u => u.GroupID == groupId);
            if (groupId == -1)
                group = new Group() { GroupID = -1 };
            return View(group);
        }

        [HttpPost]
        public ActionResult Save(Group group)
        {
            if (ModelState.IsValid)
            {
                if (group.GroupID == -1)
                {
                    Db.Groups.InsertOnSubmit(group);
                }
                else
                {
                    var cachedGroup = Db.Groups.First(u => u.GroupID == group.GroupID);
                    cachedGroup.GroupName = group.GroupName;
                    cachedGroup.CoachID = group.CoachID;
                }
                Db.SubmitChanges();
            }
            else
            {
                return View("Info", group);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int groupId)
        {
            var cachedGroup = Db.Groups.First(g => g.GroupID == groupId);
            if (cachedGroup != null)
            {
                Db.Groups.DeleteOnSubmit(cachedGroup);
                Db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}