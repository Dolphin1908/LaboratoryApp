using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.Authentication;
using LaboratoryApp.src.Data.Providers.Common;
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
        private readonly IAuthenticationService _authService;

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

        public AuthenticationViewModel(IAuthenticationService authService)
        {

            _authService = authService;

            #region Commands
            LoginCommand = new RelayCommand<object>(p => true, p =>
            {
                _ = OnLogin();
            });

            RegisterCommand = new RelayCommand<object>(p => true, p =>
            {
                _ = OnRegister();
            });

            ForgotPasswordCommand = new RelayCommand<object>(p => true, p =>
            {

            });
            #endregion
        }

        private async Task OnLogin()
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
                return;
            }
        }

        private async Task OnRegister()
        {
            // Logic for registration
            try
            {
                var isRegister = await _authService.RegisterAsync(Username, Password, ConfirmPassword, Email, PhoneNumber);
                if (!isRegister) return;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Đăng ký thành công, bạn có thể đăng nhập ngay bây giờ", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearFields();
        }

        private void OnForgotPassword()
        {
            // Logic for forgot password
        }

        private void ClearFields()
        {
            Username = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
        }
    }
}
