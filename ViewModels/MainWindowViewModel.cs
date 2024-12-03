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

namespace LaboratoryApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
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
        public ICommand NavigateToCalculatorCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService"></param>
        public MainWindowViewModel(INavigationService navigationService)
        {
            ControlBarVM = new ControlBarViewModel(this);

            _navigationService = navigationService;

            // Navigate to the welcome page when the application starts
            _navigationService.NavigateTo("../Views/UI/Welcome.xaml");

            // Navigate to the dashboard page
            NavigateToDashboardCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo("../Views/UI/Dashboard.xaml");
            });

            // Navigate to the periodic table page
            NavigateToPeriodicTableCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo("../Views/UI/PeriodicTable.xaml");
            });

            // Navigate to the compound page
            NavigateToCompoundCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo("../Views/UI/Compound.xaml");
            });

            // Navigate to the calculator page
            NavigateToCalculatorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo("../Views/UI/Calculator.xaml");
            });
        }
    }
}
