using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.DTOs.Users;
using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Data.Providers.Authentication.Interface;
using LaboratoryApp.src.Data.Providers.Common;
using LaboratoryApp.src.Services.Authentication.UserOrganizationFunction;
using LaboratoryApp.src.Services.Helper.Counter;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Windows;

namespace LaboratoryApp.src.Services.Authentication.Common
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserProvider _userProvider;
        private readonly ICounterService _counterService;
        private readonly IUserOrganizationService _userOrganizationService;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IMongoDBProvider _mongoDb;

        public AuthenticationService(IUserProvider userProvider,
                                     ICounterService counterService,
                                     IUserOrganizationService userOrganizationService,
                                     IRefreshTokenProvider refreshTokentProvider,
                                     IEnumerable<IMongoDBProvider> mongoDb)
        {
            _userProvider = userProvider;
            _counterService = counterService;
            _userOrganizationService = userOrganizationService;
            _refreshTokenProvider = refreshTokentProvider;
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);
        }

        /// <summary>
        /// Handle user registration
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<bool> RegisterAsync(string username, string password, string confirmPassword, string email, string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(confirmPassword) ||
                    string.IsNullOrWhiteSpace(email) ||
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

                var existingUsername = await _userProvider.GetUserByUsernameAsync(username);
                var existingEmail = await _userProvider.GetUserByEmailAsync(email);
                var existingPhoneNumber = await _userProvider.GetUserByPhoneNumberAsync(phoneNumber);

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
                    Id = _counterService.GetNextId(CollectionName.Users),
                    Username = username,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = SecureConfigHelper.Encrypt(password),
                };

                await _userProvider.CreateNewUserAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Xử lý đăng nhập
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LoginResultDTO> AuthenticateAsync(string username, string password)
        {
            var user = await _userProvider.GetUserByUsernameAsync(username);
            if (user == null || SecureConfigHelper.Encrypt(password) != user.Password)
            {
                return new LoginResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = "Sai thông tin đăng nhập"
                };
            }

            var (access, expires) = JwtTokenHelper.GenerateToken(user);

            var refresh = GenerateRefreshToken(user);
            await _refreshTokenProvider.UpdateAsync(refresh);

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive,
            };

            var userOrganizationDTOs = await _userOrganizationService.GetUserOrganizationDTOsByUserIdAsync(user.Id); // Lấy tất cả userOrganizationDTOs

            return new LoginResultDTO
            {
                IsSuccess = true,
                User = userDTO,
                AccessToken = access,
                RefreshToken = refresh.Token,
                OrganizationProfiles = userOrganizationDTOs
            };
        }

        /// <summary>
        /// Lưu thông tin đăng nhập vào cache
        /// </summary>
        /// <param name="loginResultDTO"></param>
        /// <param name="userOrganizationDTO"></param>
        public void SetSession(LoginResultDTO loginResultDTO, UserOrganizationProfileDTO userOrganizationDTO)
        {
            AuthenticationCache.Set(new AuthenticationResponseDTO
            {
                User = loginResultDTO.User!,
                AccessToken = loginResultDTO.AccessToken!,
                RefreshToken = loginResultDTO.RefreshToken!,
                CurrentOrganizationProfile = userOrganizationDTO,
                OrganizationProfiles = loginResultDTO.OrganizationProfiles
            });
        }

        /// <summary>
        /// Generate a new refresh token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private RefreshToken GenerateRefreshToken(User user)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return new RefreshToken
            {
                Id = _counterService.GetNextId(CollectionName.RefreshTokens),
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
