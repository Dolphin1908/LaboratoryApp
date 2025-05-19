using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using LaboratoryApp.src.UI.Views;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Modules.Toolkits.Common.Views;
using LaboratoryApp.src.Modules.Toolkits.Common.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels;
using LaboratoryApp.src.Modules.Authentication.Views;

namespace LaboratoryApp.src.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private DashboardViewModel _dashboardVM;
        private ToolkitsViewModel _toolkitsVM;
        private bool _isNavigationVisible;
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

        #region Commands
        public ICommand NavigateToDashboardCommand { get; set; }
        public ICommand OpenPeriodicTableCommand { get; set; }
        public ICommand NavigateToToolkitCommand { get; set; }
        public ICommand OpenAuthenticationCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService"></param>
        public MainWindowViewModel(INavigationService navigationService)
        {
            ControlBarVM = new ControlBarViewModel(this);

            _navigationService = navigationService;

            // Initialize the dashboard view model
            _dashboardVM = new DashboardViewModel(_navigationService);
            _toolkitsVM = new ToolkitsViewModel();

            // Initialize the dashboard page with the dashboard view model
            var dashboardPage = new Dashboard
            {
                DataContext = _dashboardVM
            };

            var toolkitsPage = new ToolkitsMainPage
            {
                DataContext = _toolkitsVM
            };

            // Navigate to the welcome page when the application starts
            _navigationService.NavigateTo(dashboardPage);

            // Navigate to the dashboard page
            NavigateToDashboardCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(dashboardPage);
            });

            // Navigate to the periodic table page
            OpenPeriodicTableCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var periodicWindow = new PeriodicTableWindow
                {
                    DataContext = new PeriodicTableViewModel()
                };
                periodicWindow.Show();
            });

            // Navigate to the toolkits page
            NavigateToToolkitCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(toolkitsPage);
            });

            // Open the authentication window
            OpenAuthenticationCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var authenticationWindow = new AuthenticationWindow();
                authenticationWindow.Show();
            });
        }
    }
}
