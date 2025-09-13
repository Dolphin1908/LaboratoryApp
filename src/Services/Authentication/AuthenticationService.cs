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
        private readonly IUserRoleProvider _userRoleProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IMongoDBProvider _db;

        public AuthenticationService(IUserProvider userProvider, 
                                     IRoleProvider roleProvider, 
                                     IUserRoleProvider userRoleProvider,
                                     IRefreshTokenProvider refreshTokentProvider,
                                     IMongoDBProvider db)
        {
            _userProvider = userProvider;
            _roleProvider = roleProvider;
            _userRoleProvider = userRoleProvider;
            _refreshTokenProvider = refreshTokentProvider;
            _db = db;
        }

        public async Task<bool> RegisterAsync(string username, string password, string confirmPassword, string email, string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("All fields are required");
                    return false;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match");
                    return false;
                }

                var existingUsername = await _userProvider.GetByUsernameAsync(username);
                var existingEmail = await _userProvider.GetByEmailAsync(email);
                var existingPhoneNumber = await _userProvider.GetByPhoneNumberAsync(phoneNumber);

                if (existingUsername != null)
                {
                    MessageBox.Show("Username already exists");
                    return false;
                }
                else if (existingEmail != null)
                {
                    MessageBox.Show("Email already exists");
                    return false;
                }
                else if (existingPhoneNumber != null)
                {
                    MessageBox.Show("Phone number already exists");
                    return false;
                }

                var user = new User
                {
                    Id = _userProvider.GetNextUserId(),
                    Username = username,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = SecureConfigHelper.Encrypt(password),
                };

                AddUser(user);

                var userRole = new UserRole
                {
                    Id = _userRoleProvider.GetNextUserRoleId(),
                    User = user,
                    Role = _roleProvider.GetRoleById(1),
                    IsActive = true,
                };

                AddUserRole(userRole);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
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

                var userRole = await _userRoleProvider.GetUserRolesAsync(user.Id);

                AuthenticationCache.Set(user, access, refresh.Token, userRole.Role.Id);

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
