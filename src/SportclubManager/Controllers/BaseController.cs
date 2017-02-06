using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using SportclubManager.Auth;
using SportclubManager.Models;

namespace SportclubManager.Controllers
{
    public abstract class BaseController : Controller
    {
        [Inject]
        public SportclubManagerDataContext Db { get; set; }

        [Inject]
        public IAuthentication Auth { get; set; }

        protected static string ErrorPage = "~/Error";
        protected static string NotFoundPage = "~/NotFoundPage";
        protected static string LoginPage = "~/Login";
        public RedirectResult RedirectToNotFoundPage
        {
            get
            {
                return Redirect(NotFoundPage);
            }
        }
        public RedirectResult RedirectToLoginPage
        {
            get
            {
                return Redirect(LoginPage);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            filterContext.Result = Redirect(ErrorPage);
        }
    }
}