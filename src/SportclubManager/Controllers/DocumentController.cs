using System;
using System.Collections.Generic;
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
                }
                else
                {
                    var cachedDoc = Db.Documents.First(d => d.DocumentID == document.DocumentID);
                    if (cachedDoc != null)
                    {
                        cachedDoc.DocumentName = document.DocumentName;
                        cachedDoc.DocumentLocation = document.DocumentLocation;
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
    }
}