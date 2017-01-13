using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SportclubManager.Models;

namespace SportclubManager.Auth
{
    /// <summary>
    /// Implementation of user identification
    /// </summary>
    public class UserIndentity : IUserIdentity
    {
        /// <summary>
        /// Current user
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// User class type
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return typeof(User).ToString();
            }
        }

        /// <summary>
        /// Is user authentificated
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        /// <summary>
        /// Unique user name
        /// </summary>
        public string Name
        {
            get
            {
                if (User != null)
                {
                    return $"{User.FirstName} {User.LastName}";
                }
                //default
                return "anonym";
            }
        }

        /// <summary>
        /// Initialize by name
        /// </summary>
        public void Init(string login, SportclubManagerDataContext dataContext)
        {
            if (!string.IsNullOrEmpty(login))
            {
                User = dataContext.Users.FirstOrDefault(
                        u => string.Compare(u.UserLogin, login, StringComparison.OrdinalIgnoreCase) == 0);
            }
        }
    }
}