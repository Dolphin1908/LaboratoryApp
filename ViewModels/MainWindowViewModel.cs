using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using LaboratoryApp.Support.Interface;
using LaboratoryApp.ViewModels.Chemistry.PeriodicTableFunction.SubWin;
using LaboratoryApp.ViewModels.UC;
using LaboratoryApp.ViewModels.UI;
using LaboratoryApp.Views.Chemistry.PeriodicTableFunction.SubWin;
using LaboratoryApp.Views.SubWin;
using LaboratoryApp.Views.UI;

namespace LaboratoryApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private DashboardViewModel _dashboardVM;
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

            // Initialize the dashboard page with the dashboard view model
            var dashboardPage = new Dashboard
            {
                DataContext = _dashboardVM
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
                _navigationService.NavigateTo(new Toolkits());
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
