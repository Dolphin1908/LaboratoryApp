using LaboratoryApp.Domain.Enums.Users;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Auth.ViewModels;
using LaboratoryApp.src.Modules.Auth.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Teacher.DashboardFunction.Views;
using LaboratoryApp.src.Modules.Tools.Toolkits.Dashboard.Views;
using LaboratoryApp.src.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigateService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;

        private bool _isNavigationVisible;

        #region Properties
        public string CurrentUser
        {
            get => AuthenticationCache.CurrentAuthentication?.User.Username ?? "Guest"; // Get the current user's username or "Guest" if not authenticated
            set
            {
                // This property is read-only, so we don't need to set it.
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public bool IsAuthenticated
        {
            get => AuthenticationCache.IsAuthenticated; // Check if the user is authenticated
            set
            {
                // This property is read-only, so we don't need to set it.
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }
        public bool IsNavigationVisible
        {
            get => _isNavigationVisible;
            set
            {
                _isNavigationVisible = value;
                OnPropertyChanged(); // Notify the UI about the change
            }
        }
        public bool IsTeacher
        {
            get => AuthenticationCache.CurrentAuthentication?.CurrentOrganizationProfile?.Role.HasFlag(UserRole.Instructor) ?? false;

            set
            {
                OnPropertyChanged(nameof(IsTeacher));
            }
        }
        public bool IsAdmin
        {
            get => AuthenticationCache.CurrentAuthentication?.CurrentOrganizationProfile?.Role.HasFlag(UserRole.OrganizationAdmin) ?? false;
            set
            {
                OnPropertyChanged(nameof(IsAdmin));
            }
        }
        public bool CanSwitchOrganization
        {
            get => AuthenticationCache.CurrentAuthentication != null && AuthenticationCache.CurrentAuthentication.OrganizationProfiles.Count > 1;
        }
        public ControlBarViewModel ControlBarVM { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateToDashboardCommand { get; set; }
        public ICommand NavigateToTeacherMainPageCommand { get; set; }
        public ICommand NavigateToTeacherToolboxCommand { get; set; }
        public ICommand NavigateToToolkitCommand { get; set; }
        public ICommand OpenPeriodicTableCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand OpenAuthenticationCommand { get; set; }
        public ICommand OpenSwitchOrganizationCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService"></param>
        public MainWindowViewModel(INavigateService navigationService,
                                   IServiceProvider serviceProvider,
                                   IDialogService dialogService)
        {
            ControlBarVM = new ControlBarViewModel(this, navigationService);

            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;

            // Navigate to the dashboard page
            NavigateToDashboardCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<Dashboard>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the teacher dashboard page
            NavigateToTeacherMainPageCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<TeacherMainPage>();
                _navigationService.NavigateTo(page);
            });

            NavigateToTeacherToolboxCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<TeacherToolsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the toolkits page
            NavigateToToolkitCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<ToolkitsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the periodic table page
            OpenPeriodicTableCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<PeriodicTableWindow>();
                window.Show();
                if (window.DataContext is IAsyncInitializable init)
                {
                    _ = init.InitializeAsync(); // Ensure the periodic table data is loaded
                }
            });

            // Logout command
            LogoutCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (MessageBoxResult.OK == _dialogService.ShowMessage("Bạn muốn đăng xuất?", "Đăng xuất", MessageBoxButton.OKCancel, MessageBoxImage.Warning))
                {
                    _navigationService.NavigateToAndClearHistory(_serviceProvider.GetRequiredService<Dashboard>());
                    // Clear authentication cache
                    AuthenticationCache.Clear();

                    // Update the current user and authentication status
                    OnAuthenticationChanged();

                    // Open the authentication window for re-login
                    var authenticationWindow = _serviceProvider.GetRequiredService<AuthenticationWindow>();
                    _dialogService.ShowDialogCenterOwner(authenticationWindow);

                    // After authentication, update the current user
                    OnAuthenticationChanged();
                }
            });

            // Open the authentication window
            OpenAuthenticationCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var authenticationWindow = _serviceProvider.GetRequiredService<AuthenticationWindow>();
                _dialogService.ShowDialogCenterOwner(authenticationWindow);

                // After authentication, update the current user
                OnAuthenticationChanged();
            });

            // Open switch organization window
            OpenSwitchOrganizationCommand = new RelayCommand<object>((p) => CanSwitchOrganization, (p) =>
            {
                if (MessageBoxResult.OK == _dialogService.ShowMessage("Bạn muốn đổi vai trò khác?", "Đổi vai trò", MessageBoxButton.OKCancel, MessageBoxImage.Warning))
                {
                    _navigationService.NavigateToAndClearHistory(_serviceProvider.GetRequiredService<Dashboard>());

                    var switchOrgWindow = _serviceProvider.GetRequiredService<RoleSelectionWindow>();
                    var switchOrgVm = switchOrgWindow.DataContext as RoleSelectionViewModel;
                    switchOrgVm!.Roles = AuthenticationCache.CurrentAuthentication!.OrganizationProfiles;
                    _dialogService.ShowDialogCenterOwner(switchOrgWindow);
                    // After switching organization, update the properties
                    OnAuthenticationChanged();
                }
            });
        }

        private void OnAuthenticationChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsTeacher));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(CanSwitchOrganization));
        }

        /// <summary>
        /// Khởi tạo trang chính
        /// </summary>
        public void Initialize()
        {
            var dashboardPage = _serviceProvider.GetRequiredService<Dashboard>();
            _navigationService.NavigateTo(dashboardPage);
            OnAuthenticationChanged();
        }
    }
}
