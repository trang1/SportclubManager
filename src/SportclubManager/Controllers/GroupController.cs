using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class GroupController : BaseController
    {
        // GET: Group
        public ActionResult Index()
        {
            var groups = Db.Groups.OrderBy(u => u.GroupID).ToList();
            return View(groups);
        }

        public ActionResult Info(int groupId)
        {
            var group = Db.Groups.FirstOrDefault(u => u.GroupID == groupId);
            if (groupId == -1)
                group = new Group() { GroupID = -1 };
            return View(group);
        }

        [HttpPost]
        public ActionResult Save(Group @group)
        {
            if (@group != null)
            {
                if (@group.GroupID == -1)
                {
                    Db.Groups.InsertOnSubmit(@group);
                }
                else
                {
                    var cachedGroup = Db.Groups.First(u => u.GroupID == @group.GroupID);
                    if (cachedGroup != null)
                    {
                        cachedGroup.GroupName = @group.GroupName;
                        cachedGroup.User = Db.Users.FirstOrDefault(r => r.UserID == int.Parse(@group.SelectedCoachValue));
                    }
                }
                Db.SubmitChanges();
            }
            else
            {

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