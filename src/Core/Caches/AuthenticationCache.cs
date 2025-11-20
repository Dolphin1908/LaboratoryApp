using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.DTOs.Users;

namespace LaboratoryApp.src.Core.Caches
{
    public static class AuthenticationCache
    {
        private static AuthenticationResponseDTO? _currentAuthentication;

        public static event Action<AuthenticationResponseDTO?>? CurrentAuthenticationChanged;

        public static AuthenticationResponseDTO? CurrentAuthentication
        {
            get => _currentAuthentication;
            private set
            {
                if (_currentAuthentication != value)
                {
                    _currentAuthentication = value;
                    CurrentAuthenticationChanged?.Invoke(_currentAuthentication);
                }
            }
        }

        public static bool IsAuthenticated => CurrentAuthentication != null;

        public static void Set(UserDTO user, string accessToken, string refreshToken)
        {
            CurrentAuthentication = new AuthenticationResponseDTO
            {
                User = user,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }
        public static void Set(AuthenticationResponseDTO authenticationResponseDTO)
        {
            CurrentAuthentication = authenticationResponseDTO;
        }

        public static void Clear()
        {
            CurrentAuthentication = null;
        }
    }
}
