using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SportclubManager.Auth;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class MemberController : BaseController
    {
        // GET: Member
        public ActionResult Index()
        {
            var members = Db.Members.AsQueryable();

            if (UserProvider.CurrentUser.IsCoach)
            {
                var groupIds = Db.Groups.Where(g => g.CoachID == UserProvider.CurrentUser.UserID).Select(g=>g.GroupID).ToList();
                members = members.Where(d => d.GroupID.HasValue && groupIds.Contains(d.GroupID.Value));
            }
            return View(members.ToList());
        }

        public ActionResult Info(int memberId)
        {
            var member = Db.Members.FirstOrDefault(m => m.MemberID == memberId);
            if (memberId == -1)
                member = new Member() { MemberID = -1, IsActive = true};
            return View(new MemberModel(member));
        }

        [HttpPost]
        public ActionResult Save(MemberModel memberModel)
        {
            if (ModelState.IsValid)
            {
                var member = memberModel.GetMember();
                if (member.MemberID == -1)
                {
                    Db.Members.InsertOnSubmit(member);
                    
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file.ContentLength > 0)
                        {
                            // extract only the filename
                            var fileName = Path.GetFileName(file.FileName);

                            // store the file inside folder
                            const string location = "~/App_Data/uploads/members";

                            var path = Path.Combine(Server.MapPath(location), fileName);
                            file.SaveAs(path);

                            member.PhotoLocation = location + "/" + fileName;
                        }
                    }
                }
                else
                {
                    var cachedMember = Db.Members.First(d => d.MemberID == member.MemberID);
                    if (cachedMember != null)
                    {
                        cachedMember.FirstName = member.FirstName;
                        cachedMember.LastName = member.LastName;
                        cachedMember.Address = member.Address;
                        cachedMember.IsActive = member.IsActive;
                        cachedMember.DOB = member.DOB;
                        cachedMember.Father = member.Father;
                        cachedMember.FatherMail = member.FatherMail;
                        cachedMember.FatherPhoneNo = member.FatherPhoneNo;
                        cachedMember.Mother = member.Mother;
                        cachedMember.MotherMail = member.MotherMail;
                        cachedMember.MotherPhoneNo = member.MotherPhoneNo;
                        cachedMember.GroupID = member.GroupID;

                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[0];
                            if (file.ContentLength > 0)
                            {
                                // extract only the filename
                                var fileName = Path.GetFileName(file.FileName);
                                // store the file inside folder
                                const string location = "~/App_Data/uploads/members";

                                var path = Path.Combine(Server.MapPath(location), fileName);
                                file.SaveAs(path);

                                cachedMember.PhotoLocation = location + "/" + fileName;
                            }
                        }
                    }
                }
                Db.SubmitChanges();
            }
            else
            {
                return View("Info", memberModel);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int memberId)
        {
            var cachedMember = Db.Members.First(d => d.MemberID== memberId);
            if (cachedMember != null)
            {
                Db.Members.DeleteOnSubmit(cachedMember);
                Db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult OpenPhoto(string location)
        {
            return File(Server.MapPath(location), "application/text", Path.GetFileName(location));
        }

        public JsonResult DeletePhoto(string location)
        {
            var path = Server.MapPath(location);
            try //Maybe error could happen like Access denied or Presses Already User used
            {
                System.IO.File.Delete(path);

                var members = Db.Members.Where(m => m.PhotoLocation == location).ToList();
                members.ForEach(m => m.PhotoLocation = null);
                Db.SubmitChanges();
            }
            catch (Exception e)
            {
                return Json(new {Success = false, Message = "Error. " + e.Message});
            }

            return Json(new {Success = true, Message = "Successfully deleted!"}, 
                JsonRequestBehavior.AllowGet);
        }
    }
}