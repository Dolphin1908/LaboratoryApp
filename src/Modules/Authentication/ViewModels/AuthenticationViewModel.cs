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
            var refreshTokenProvider = new RefreshTokenProvider(dbProvider);

            _authService = new AuthenticationService(userProvider, refreshTokenProvider);

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

        private void OnLogin()
        {
            // Logic for login

            MessageBox.Show($"{Username}, {Password}");
        }

        private void OnRegister()
        {
            // Logic for registration

            MessageBox.Show($"{Username}, {Password}");
        }

        private void OnForgotPassword()
        {
            // Logic for forgot password
        }
    }
}
