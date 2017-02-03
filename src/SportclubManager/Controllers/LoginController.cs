using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginModel());
        }
        [HttpPost]
        public ActionResult Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginModel.Login, loginModel.Password, loginModel.IsPersistent);
                if (user != null)
                {
                    switch (user.Role.RoleName)
                    {
                        case Roles.Administrator:
                            return RedirectToAction("Index", "Admin");

                        case Roles.Coach:
                            return RedirectToAction("Index", "Coach");
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState["Password"].Errors.Add("Wrong login or password specified.");
            }
            return View(loginModel);
        }
        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}