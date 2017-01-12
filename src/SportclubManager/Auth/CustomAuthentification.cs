using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Ninject;
using NLog;
using SportclubManager.Models;

namespace SportclubManager.Auth
{
    public class CustomAuthentication : IAuthentication 
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string CookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext { get; set; }

        [Inject]
        public SportclubManagerDataContext DataContext { get; set; }

        #region IAuthentication Members

        public User Login(string userName, string password, bool isPersistent)
        {
            var user = DataContext.Users.FirstOrDefault(
                    p => string.Compare(p.UserLogin, userName, StringComparison.OrdinalIgnoreCase) == 0 &&
                         string.Compare(p.UserPassword, password, StringComparison.OrdinalIgnoreCase) == 0);
            if (user != null)
            {
                CreateCookie(userName, isPersistent);
            }
            return user;
        }

        public User Login(string userName)
        {
            User retUser = DataContext.Users.FirstOrDefault(p => string.Compare(p.UserLogin, userName, StringComparison.OrdinalIgnoreCase) == 0);
            if (retUser != null)
            {
                CreateCookie(userName);
            }
            return retUser;
        }

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                  1,
                  userName,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var authCookie = new HttpCookie(CookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authCookie);
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[CookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private IPrincipal _currentUser;

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = HttpContext.Request.Cookies.Get(CookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(ticket.Name, DataContext);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Failed authentication: " + ex.Message);
                        _currentUser = new UserProvider(null, null);
                    }
                }
                return _currentUser;
            }
        }
        #endregion
    }
}