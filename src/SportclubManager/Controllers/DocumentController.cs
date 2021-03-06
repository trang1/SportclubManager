﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Auth;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class DocumentController : BaseController
    {
        // GET: Document
        public ActionResult Index()
        {
            if (UserProvider.CurrentUser == null)
                return RedirectToAction("Home", "Home");

            var docs = Db.Documents.AsQueryable();

            //•	Users with role Coach should be able to see other persons documents, but only delete their own. 
            //if (UserProvider.CurrentUser.IsCoach)
            //    docs = docs.Where(d => d.UserID == UserProvider.CurrentUser.UserID);

            return View(docs.ToList());
        }

        public ActionResult Info(int docId)
        {
            if (UserProvider.CurrentUser == null)
                return RedirectToAction("Home", "Home");

            var doc = Db.Documents.FirstOrDefault(d=>d.DocumentID == docId);
            if (docId == -1)
                doc = new Document() { DocumentID = -1 };
            return View(doc);
        }

        [HttpPost]
        public ActionResult Save(Document document)
        {
            if (ModelState.IsValid)
            {
                if (document.DocumentID == -1)
                {
                    Db.Documents.InsertOnSubmit(document);

                    document.UserID = UserProvider.CurrentUser.UserID;

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
                        cachedDoc.UserID = UserProvider.CurrentUser.UserID;

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
                return View("Info", document);
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

        public JsonResult DeleteFile(string location)
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
                return Json(new { Success = false, Message = "Error. " + e.Message });
            }

            return Json(new { Success = true, Message = "Successfully deleted!" },
                JsonRequestBehavior.AllowGet);
        }
    }
}