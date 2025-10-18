using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;

namespace LaboratoryApp.src.Core.Caches
{
    public static class AuthenticationCache
    {
        private static UserDTO? _currentUser;
        private static string? _accessToken;
        private static string? _refreshToken;

        public static event Action<UserDTO?>? CurrentUserChanged;

        public static UserDTO? CurrentUser
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

        public static bool IsAuthenticated =>
            CurrentUser != null &&
            !string.IsNullOrEmpty(AccessToken) &&
            !string.IsNullOrEmpty(RefreshToken);

        public static void Set(UserDTO user, string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            CurrentUser = user;
        }

        public static void Clear()
        {
            AccessToken = null;
            RefreshToken = null;
            CurrentUser = null;
        }
    }
}
