using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SportclubManager.Models;

namespace SportclubManager.Auth
{
    /// <summary>
    /// Реализация интерфейса Principal
    /// </summary>
    public class UserProvider : IPrincipal
    {
        private UserIndentity _userIndentity;

        #region IPrincipal Members

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return _userIndentity;
            }
        }

        /// <summary>
        /// Находится в данной роли или нет
        /// </summary>
        /// <param name="role">имя роли</param>
        /// <returns>есть такая роль или нет</returns>
        public bool IsInRole(string role)
        {
            if (_userIndentity.User == null)
            {
                return false;
            }
            return _userIndentity.User.InRoles(role);
        }

        #endregion

        /// <summary>
        /// конструктор  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="repository"></param>
        public UserProvider(string name, SportclubManagerDataContext repository)
        {
            _userIndentity = new UserIndentity();
            _userIndentity.Init(name, repository);
        }


        /// <summary>
        /// User Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _userIndentity.Name;
        }

        public static User CurrentUser
        {
            get
            {
                var auth = DependencyResolver.Current.GetService<IAuthentication>();
                return ((IUserIdentity) auth.CurrentUser.Identity).User;
            }
        }
    }
}