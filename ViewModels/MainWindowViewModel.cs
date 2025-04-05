using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using LaboratoryApp.Support.Interface;
using LaboratoryApp.ViewModels.UC;
using LaboratoryApp.ViewModels.UI;
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

        #region commands
        public ICommand NavigateToDashboardCommand { get; set; }
        public ICommand NavigateToPeriodicTableCommand { get; set; }
        public ICommand NavigateToCompoundCommand { get; set; }
        public ICommand NavigateToToolkitCommand { get; set; }
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
            NavigateToPeriodicTableCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(new PeriodicTable());
            });

            // Navigate to the compound page
            NavigateToCompoundCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(dashboardPage);
            });

            // Navigate to the toolkits page
            NavigateToToolkitCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(new Toolkits());
            });
        }
    }
}
