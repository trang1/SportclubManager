using System.Web.Mvc;
using Ninject;
using NLog;
using SportclubManager.Auth;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static string ErrorPage = "~/Error";
        protected static string NotFoundPage = "~/Error/NotFoundPage";
        protected static string LoginPage = "~/Login";

        [Inject]
        public SportclubManagerDataContext Db { get; set; }

        [Inject]
        public IAuthentication Auth { get; set; }

        public RedirectResult RedirectToNotFoundPage
        {
            get { return Redirect(NotFoundPage); }
        }

        public RedirectResult RedirectToLoginPage
        {
            get { return Redirect(LoginPage); }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            LogManager.GetCurrentClassLogger().Error(filterContext.Exception, filterContext.Controller.GetType().Name);
            filterContext.Result = Redirect(ErrorPage);
        }
    }
}