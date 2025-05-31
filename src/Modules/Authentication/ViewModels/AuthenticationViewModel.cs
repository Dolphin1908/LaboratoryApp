using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.Authentication;
using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Services.Authentication;
using LaboratoryApp.src.Core.Helpers;
using System.Windows.Input;
using System.Windows;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.UI.Views;
using LaboratoryApp.src.Modules.Authentication.Views;

namespace LaboratoryApp.src.Modules.Authentication.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authService;

        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _email;
        private string _phoneNumber;

        #region Commands
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }
        #endregion

        #region Properties
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        #endregion

        public AuthenticationViewModel()
        {
            var conn = SecureConfigHelper.Decrypt(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
            var dbProvider = new MongoDBProvider(conn, "authentication");
            var userProvider = new UserProvider(dbProvider);
            var roleProvider = new RoleProvider(dbProvider);
            var refreshTokenProvider = new RefreshTokenProvider(dbProvider);

            _authService = new AuthenticationService(userProvider, roleProvider, refreshTokenProvider);

            #region Commands
            LoginCommand = new RelayCommand<object>(p => true, p =>
            {
                OnLogin();
            });

            RegisterCommand = new RelayCommand<object>(p => true, p =>
            {
                OnRegister();
            });

            ForgotPasswordCommand = new RelayCommand<object>(p => true, p =>
            {

            });
            #endregion
        }

        private async void OnLogin()
        {
            // Logic for login
            await _authService.LoginAsync(Username, Password);

            if (AuthenticationCache.CurrentUser != null)
            {
                var currentWindow = Application.Current.Windows.OfType<AuthenticationWindow>().FirstOrDefault();
                if (currentWindow != null)
                {
                    currentWindow.Close();
                }
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại, vui lòng kiểm tra lại thông tin", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void OnRegister()
        {
            // Logic for registration
            await _authService.RegisterAsync(Username, Password, ConfirmPassword, Email, PhoneNumber);
        }

        private void OnForgotPassword()
        {
            // Logic for forgot password
        }
    }
}
