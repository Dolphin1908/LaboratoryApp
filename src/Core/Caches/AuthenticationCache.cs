using LaboratoryApp.src.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Caches
{
    public static class AuthenticationCache
    {
        public static User? CurrentUser { get; private set; }
        public static string? AccessToken { get; private set; }
        public static string? RefreshToken { get; private set; }
        public static bool IsAuthenticated => CurrentUser != null && !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(RefreshToken);

        public static void Set(User user, string accessToken, string refreshToken)
        {
            CurrentUser = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public static void Clear()
        {
            CurrentUser = null;
            AccessToken = null;
            RefreshToken = null;
        }
    }
}
