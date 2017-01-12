using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SportclubManager.Models;

namespace SportclubManager.Auth
{
    /// <summary>
    /// Реализация интерфейса для идентификации пользователя
    /// </summary>
    public class UserIndentity : IUserIdentity
    {
        /// <summary>
        /// Текщий пользователь
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Тип класса для пользователя
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return typeof(User).ToString();
            }
        }

        /// <summary>
        /// Авторизован или нет
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        /// <summary>
        /// Имя пользователя (уникальное) [у нас это счас Email]
        /// </summary>
        public string Name
        {
            get
            {
                if (User != null)
                {
                    return $"{User.FirstName} {User.LastName}";
                }
                //иначе аноним
                return "anonym";
            }
        }

        /// <summary>
        /// Инициализация по имени
        /// </summary>
        /// <param name="email">имя пользователя [email]</param>
        public void Init(string email, SportclubManagerDataContext dataContext)
        {
            if (!string.IsNullOrEmpty(email))
            {
                User = dataContext.Users.FirstOrDefault(
                        u => string.Compare(u.UserLogin, email, StringComparison.OrdinalIgnoreCase) == 0);
            }
        }
    }
}