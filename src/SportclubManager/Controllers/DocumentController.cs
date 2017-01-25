using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class DocumentController : BaseController
    {
        // GET: Document
        public ActionResult Index()
        {
            var docs = Db.Documents.AsQueryable();

            if (CurrentUser.IsCoach)
                docs = docs.Where(d => d.UserID == CurrentUser.UserID);

            return View(docs.ToList());
        }

        public ActionResult Info(int docId)
        {
            var doc = Db.Documents.FirstOrDefault(d=>d.DocumentID == docId);
            if (docId == -1)
                doc = new Document() { DocumentID = -1 };
            return View(doc);
        }

        [HttpPost]
        public ActionResult Save(Document document)
        {
            if (document != null)
            {
                if (document.DocumentID == -1)
                {
                    Db.Documents.InsertOnSubmit(document);

                    document.UserID = CurrentUser.UserID;

                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file.ContentLength > 0)
                        {
                            // extract only the filename
                            var fileName = Path.GetFileName(file.FileName);

                            // store the file inside ~/App_Data/uploads/documents folder
                            const string location = "~/App_Data/uploads/documents";

                            var path = Path.Combine(Server.MapPath(location), fileName);
                            file.SaveAs(path);

                            document.DocumentLocation = location + "/" + fileName;
                        }
                    }
                }
                else
                {
                    var cachedDoc = Db.Documents.First(d => d.DocumentID == document.DocumentID);
                    if (cachedDoc != null)
                    {
                        cachedDoc.DocumentName = document.DocumentName;
                        cachedDoc.UserID = CurrentUser.UserID;

                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[0];
                            if (file.ContentLength > 0)
                            {
                                // extract only the filename
                                var fileName = Path.GetFileName(file.FileName);
                                // store the file inside ~/App_Data/uploads/documents folder
                                const string location = "~/App_Data/uploads/documents";

                                var path = Path.Combine(Server.MapPath(location), fileName);
                                file.SaveAs(path);

                                cachedDoc.DocumentLocation = location + "/" + fileName;
                            }
                        }
                    }
                }
                Db.SubmitChanges();
            }
            else
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int docId)
        {
            var cachedDoc = Db.Documents.First(d => d.DocumentID == docId);
            if (cachedDoc != null)
            {
                Db.Documents.DeleteOnSubmit(cachedDoc);
                Db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult OpenFile(string location)
        {
            return File(Server.MapPath(location), "application/text", Path.GetFileName(location));
        }

        public ActionResult DeleteFile(string location)
        {
            var path = Server.MapPath(location);
            try //Maybe error could happen like Access denied or Presses Already User used
            {
                System.IO.File.Delete(path);

                var docs = Db.Documents.Where(d => d.DocumentLocation == location).ToList();
                docs.ForEach(d=>d.DocumentLocation = null);
                Db.SubmitChanges();
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e.Message);
            }

            return null;
        }
    }
}