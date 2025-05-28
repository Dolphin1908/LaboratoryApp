using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Data.Providers.Authentication;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using System.Security.Cryptography;

namespace LaboratoryApp.src.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly UserProvider _userProvider;
        private readonly RefreshTokenProvider _refreshTokenProvider;

        public AuthenticationService(UserProvider userProvider, RefreshTokenProvider refreshTokentProvider)
        {
            _userProvider = userProvider;
            _refreshTokenProvider = refreshTokentProvider;
        }

        public async Task<AuthenticationResponseDTO?> LoginAsync(string username, string password)
        {
            var user = await _userProvider.GetByUsernameAsync(username);
            if (user == null || SecureConfigHelper.Encrypt(password) != user.Password)
                return null;

            var (access, expires) = JwtTokenHelper.GenerateToken(user);
            var refresh = GenerateRefreshToken(user);
            await _refreshTokenProvider.CreateAsync(refresh);

            return new AuthenticationResponseDTO
            {
                User = user,
                AccessToken = access,
                RefreshToken = refresh.Token,
                ExpiresAt = expires,
            };
        }

        private RefreshToken GenerateRefreshToken(User user)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return new RefreshToken
            {
                User = user,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
