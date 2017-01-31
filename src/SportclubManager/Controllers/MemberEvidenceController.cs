using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class MemberEvidenceController : BaseController
    {
        // GET: MemberEvidence
        public ActionResult Index()
        {
            var mem = new MemberEvidenceModel();

            if (mem.Groups.Any())
            {
                mem.CurrentDate = DateTime.Today;
                mem.MemberEvidences = GetMembersEvidences(mem.CurrentDate, mem.Groups.First().GroupID);
            }

            return View(mem);
        }

        private List<MemberEvidenceWeek> GetMembersEvidences(DateTime currentDate, int groupId)
        {
            var group = Db.Groups.FirstOrDefault(g => g.GroupID == groupId);
            DateTime curDate;
            var list = new List<MemberEvidenceWeek>();

            if (group != null)
            {
                int currentDayOfWeek = (int)currentDate.DayOfWeek;
                var sunday = currentDate.AddDays(-currentDayOfWeek);
                var monday = sunday.AddDays(1);
                // If we started on Sunday, we should actually have gone *back*
                // 6 days instead of forward 1...
                if (currentDayOfWeek == 0)
                {
                    monday = monday.AddDays(-7);
                }
                var days = new[] {"Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"};
                foreach (var member in group.Members)
                {
                    var mew = new MemberEvidenceWeek {MemberName = member.FullName, CurrentDate = currentDate};

                    for (var i = 0; i < 7; i++)
                    {
                        var date = monday.AddDays(i);
                        var day = days[i];

                        var me = Db.MemberEvidences.FirstOrDefault(m => m.Date == date && m.MemberID == member.MemberID);
                        if (me == null)
                        {
                            me = new MemberEvidence {Date = date, GroupID = group.GroupID, MemberID = member.MemberID};
                            Db.MemberEvidences.InsertOnSubmit(me);
                            Db.MemberEvidences.Context.SubmitChanges();
                        }
                        mew[day] = me;
                    }

                    list.Add(mew);
                }
            }
            return list;
        }

        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }

        public ActionResult AddDate(string date, int groupId)
        {
            DateTime dateTime;
            var weeks = new List<MemberEvidenceWeek>();

            if (DateTime.TryParse(date, out dateTime))
            {
                weeks = GetMembersEvidences(dateTime.Date, groupId);
            }
            return View("Evidences", weeks);
        }
    }
}