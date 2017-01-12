using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SportclubManager.Models;

namespace SportclubManager.Auth
{
    /// <summary>
    /// Interface for user authentification
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// Context to get cookies & request params
        /// </summary>
        HttpContext HttpContext { get; set; }

        /// <summary>
        /// Login procedure
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <param name="isPersistent">expirable</param>
        /// <returns></returns>
        User Login(string login, string password, bool isPersistent);

        /// <summary>
        /// Входим без пароля (использовать осторожно)
        /// </summary>
        /// <param name="login">логин</param>
        /// <returns></returns>
        User Login(string login);

        /// <summary>
        /// Logout procedure
        /// </summary>
        void LogOut();

        /// <summary>
        /// Current authorized user
        /// </summary>
        IPrincipal CurrentUser { get; }
    }
}
