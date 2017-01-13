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
        public User CurrentUser
        {
            get
            {
                return ((IUserIdentity)Auth.CurrentUser.Identity).User;
            }
        }
    }
}