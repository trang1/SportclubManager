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
                mem.MemberEvidences = new List<MemberEvidenceWeek>();
                mem.CurrentDate = DateTime.Today.ToShortDateString();

                var group = mem.Groups.First();

                int currentDayOfWeek = (int) DateTime.Today.DayOfWeek;
                var sunday = DateTime.Today.AddDays(-currentDayOfWeek);
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
                    var mew = new MemberEvidenceWeek {MemberName = member.FullName};

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

                    mem.MemberEvidences.Add(mew);
                }
            }

            return View(mem);
        }

        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }
    }
}