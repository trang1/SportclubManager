using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
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

                var groupName = report.GroupList.First(g => g.Value == report.GroupID.ToString()).Text;
                var data =
                    mes.GroupBy(m => m.Member.FullName)
                        .Select(
                            m => new
                            {
                                MemberName = m.Key,
                                Percentage =
                                    ((decimal) m.Count(me => me.Present.GetValueOrDefault())/m.Count()).ToString("P")
                            })
                        .ToList();

                if (data.Count == 0)
                    data.Add(new {MemberName = "", Percentage = ""});

                var gv = new GridView();
                gv.DataSource = data;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                    $"attachment; filename=Members ({groupName} {report.DateFrom:d} - {report.DateTo:d}).xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "UTF-8";
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return RedirectToAction("Index");
        }
    }
}