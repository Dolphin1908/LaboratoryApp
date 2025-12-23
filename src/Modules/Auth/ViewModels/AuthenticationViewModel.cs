using LaboratoryApp.Domain.DTOs.Users;
using LaboratoryApp.Domain.Enums.Users;
using LaboratoryApp.Domain.Interfaces.Services.Auth;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Auth.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Auth.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;
        private readonly IAuthenticationService _authService;

        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _email = string.Empty;
        private string _phoneNumber = string.Empty;

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

        public AuthenticationViewModel(IServiceProvider serviceProvider,
                                       IDialogService dialogService,
                                       IAuthenticationService authService)
        {
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;
            _authService = authService;

            #region Commands
            LoginCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is AuthenticationWindow window)
                {
                    _ = OnLogin(window);
                }
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

        /// <summary>
        /// Xử lý logic đăng nhập
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        private async Task OnLogin(AuthenticationWindow window)
        {
            // Xử lý logic đăng nhập
            var loginResult = await _authService.AuthenticateAsync(Username, Password);

            if (loginResult.IsSuccess == false)
            {
                _dialogService.ShowMessage(loginResult.ErrorMessage!, "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Xử lý sau khi đăng nhập thành công
            // Chọn role và organization nếu cần thiết
            UserOrganizationProfileDTO selectedRole = new UserOrganizationProfileDTO
            {
                OrganizationId = 0,
                OrganizationName = "No Organization",
                Role = UserRole.Guest
            };

            if (loginResult.OrganizationProfiles.Count == 0)
            {
                _dialogService.ShowMessage("Tài khoản của bạn chưa được phân quyền vào tổ chức nào.");
            }
            else if (loginResult.OrganizationProfiles.Count == 1)
            {
                selectedRole = loginResult.OrganizationProfiles[0];
            }
            else
            {
                // Hiển thị hộp thoại chọn role và organization
                var roleSelectionWindow = _serviceProvider.GetRequiredService<RoleSelectionWindow>();
                var windowVm = roleSelectionWindow.DataContext as RoleSelectionViewModel;
                windowVm!.Roles = loginResult.OrganizationProfiles; // Truyền danh sách role và organization vào ViewModel

                _dialogService.ShowDialogCenterOwner(roleSelectionWindow);

                selectedRole = windowVm.SelectedRole!; // Lấy role và organization đã chọn
            }

            // Lưu thông tin người dùng đã đăng nhập
            _authService.SetSession(loginResult, selectedRole!);
            window.Close(); // Đóng cửa sổ đăng nhập
        }

        /// <summary>
        /// Xử lý logic đăng ký
        /// </summary>
        /// <returns></returns>
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
                _dialogService.ShowMessage(e.Message, "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _dialogService.ShowMessage("Đăng ký thành công, bạn có thể đăng nhập ngay bây giờ", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
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
