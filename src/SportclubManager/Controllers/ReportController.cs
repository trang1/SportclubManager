using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            var rm = new ReportModel {DateFrom = DateTime.Today.AddDays(-1), DateTo = DateTime.Today};
            return View(rm);
        }

        public ActionResult Show(ReportModel report)
        {
            if (report?.GroupID != null)
            {
                var mes = Db.MemberEvidences.Where(
                        me => me.GroupID == report.GroupID && me.Date >= report.DateFrom && me.Date <= report.DateTo)
                        .ToList();
            }
            else
            {
                
            }
            return RedirectToAction("Index");
        }
    }
}