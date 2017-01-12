using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginView());
        }
        [HttpPost]
        public ActionResult Index(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Login, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState["Password"].Errors.Add("Wrong login or password specified.");
            }
            return View(loginView);
        }
        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}