using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows;

using MongoDB.Driver;

using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using LaboratoryApp.src.Data.Providers.Interfaces;

namespace LaboratoryApp.src.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserProvider _userProvider;
        private readonly IRoleProvider _roleProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IMongoDBProvider _db;

        public AuthenticationService(IUserProvider userProvider, 
                                     IRoleProvider roleProvider, 
                                     IRefreshTokenProvider refreshTokentProvider,
                                     IMongoDBProvider db)
        {
            _userProvider = userProvider;
            _roleProvider = roleProvider;
            _refreshTokenProvider = refreshTokentProvider;
            _db = db;
        }

        public async Task RegisterAsync(string username, string password, string confirmPassword, string email, string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("All fields are required");
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match");
                    return;
                }

                var existingUser = await _userProvider.GetByUsernameAsync(username);
                if (existingUser != null)
                {
                    MessageBox.Show("Username already exists");
                    return;
                }

                var userId = _userProvider.GetAllUsers().Count > 0
                    ? _userProvider.GetAllUsers().Max(u => u.Id) + 1
                    : 1;
                var user = new User
                {
                    Id = userId,
                    Username = username,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = SecureConfigHelper.Encrypt(password),
                };

                AddUser(user);

                var userRole = new UserRole
                {
                    User = user,
                    Role = _roleProvider.GetRoleById(1),
                    IsActive = true,
                };

                AddUserRole(userRole);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return;
            }
        }

        public async Task<AuthenticationResponseDTO?> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _userProvider.GetByUsernameAsync(username);
                if (user == null || SecureConfigHelper.Encrypt(password) != user.Password)
                    return null;

                var (access, expires) = JwtTokenHelper.GenerateToken(user);

                var refresh = new RefreshToken();
                var existingToken = await _refreshTokenProvider.GetLatestByUserIdAsync(user.Id);

                if (existingToken != null && existingToken.ExpiresAt > DateTime.UtcNow && existingToken.RevokedAt == null)
                {
                    refresh = existingToken;
                }
                else
                {
                    refresh = GenerateRefreshToken(user);
                    refresh.Id = _refreshTokenProvider.GetNextId();

                    if (existingToken != null)
                    {
                        existingToken.RevokedAt = DateTime.UtcNow;
                        existingToken.ReplacedBy = refresh.Token;
                        await _refreshTokenProvider.UpdateAsync(existingToken);
                    }

                    await _refreshTokenProvider.CreateAsync(refresh);
                }

                //var refresh = GenerateRefreshToken(user);
                //await _refreshTokenProvider.CreateAsync(refresh);

                AuthenticationCache.Set(user, access, refresh.Token);

                return new AuthenticationResponseDTO
                {
                    User = user,
                    AccessToken = access,
                    RefreshToken = refresh.Token,
                    ExpiresAt = expires,
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return null;
            }
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

        private void AddUser(User user)
        {
            try
            {
                _db.Insert("users", user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}");
                return;
            }
        }

        private void AddUserRole(UserRole userRole) => _db.Insert("userRoles", userRole);
    }
}
