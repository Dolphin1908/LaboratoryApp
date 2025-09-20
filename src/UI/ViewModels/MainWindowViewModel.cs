using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Toolkits.Common.Views;
using LaboratoryApp.src.Modules.Toolkits.Common.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels;
using LaboratoryApp.src.Modules.Authentication.Views;
using LaboratoryApp.src.Modules.Authentication.ViewModels;

using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.UI.Views;

namespace LaboratoryApp.src.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private bool _isNavigationVisible;

        #region Properties
        public string CurrentUser
        {
            get => AuthenticationCache.CurrentUser?.Username ?? "Guest"; // Get the current user's username or "Guest" if not authenticated
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
        public ControlBarViewModel ControlBarVM { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateToDashboardCommand { get; set; }
        public ICommand OpenPeriodicTableCommand { get; set; }
        public ICommand NavigateToToolkitCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand OpenAuthenticationCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService"></param>
        public MainWindowViewModel(INavigationService navigationService, 
                                   IServiceProvider serviceProvider)
        {
            ControlBarVM = new ControlBarViewModel(this, navigationService);

            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            // Initialize the dashboard page with the dashboard view model
            var dashboardPage = _serviceProvider.GetRequiredService<Dashboard>();

            // Navigate to the welcome page when the application starts
            _navigationService.NavigateTo(dashboardPage);

            // Navigate to the dashboard page
            NavigateToDashboardCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<Dashboard>();
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

            // Navigate to the toolkits page
            NavigateToToolkitCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<ToolkitsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Logout command
            LogoutCommand = new RelayCommand<object>((p)=>true, (p) =>
            {
                // Clear authentication cache
                AuthenticationCache.Clear();

                // Update the current user and authentication status
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsAuthenticated));

                // Open the authentication window for re-login
                var authenticationWindow = _serviceProvider.GetRequiredService<AuthenticationWindow>();
                authenticationWindow.ShowDialog();

                // After authentication, update the current user
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsAuthenticated));
            });

            // Open the authentication window
            OpenAuthenticationCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var authenticationWindow = _serviceProvider.GetRequiredService<AuthenticationWindow>();
                authenticationWindow.ShowDialog();
                
                // After authentication, update the current user
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsAuthenticated));
            });
        }
    }
}
