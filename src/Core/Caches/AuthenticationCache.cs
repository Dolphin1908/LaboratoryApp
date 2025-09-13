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
        private static User? _currentUser;
        private static string? _accessToken;
        private static string? _refreshToken;
        private static long _roleId;

        public static event Action<User?>? CurrentUserChanged;

        public static User? CurrentUser
        {
            get => _currentUser;
            private set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    CurrentUserChanged?.Invoke(_currentUser);
                }
            }
        }
        public static string? AccessToken
        {
            get => _accessToken;
            private set => _accessToken = value;
        }
        public static string? RefreshToken
        {
            get => _refreshToken;
            private set => _refreshToken = value;
        }
        public static long? RoleId
        {
            get => _roleId;
            set => _roleId = value ?? 0;
        }
        public static bool IsAuthenticated =>
            CurrentUser != null &&
            !string.IsNullOrEmpty(AccessToken) &&
            !string.IsNullOrEmpty(RefreshToken);



        public static void Set(User user, string accessToken, string refreshToken, long roleId)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RoleId = roleId;
            CurrentUser = user;
        }

        public static void Clear()
        {
            AccessToken = null;
            RefreshToken = null;
            RoleId = 0;
            CurrentUser = null;
        }
    }
}
