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
        public static long? CurrentUserId { get; private set; }
        public static string? AccessToken { get; private set; }
        public static string? RefreshToken { get; private set; }

        public static void Set(long userId, string accessToken, string refreshToken)
        {
            CurrentUserId = userId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public static void Clear()
        {
            CurrentUserId = null;
            AccessToken = null;
            RefreshToken = null;
        }
    }
}
