using LaboratoryApp.Support.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

using LaboratoryApp.Support.Interface;
using LaboratoryApp.Support;
using LaboratoryApp.Views.Maths;
using LaboratoryApp.Views.Physics;
using LaboratoryApp.Views.Chemistry;
using LaboratoryApp.Views.English;
using LaboratoryApp.ViewModels.Maths;
using LaboratoryApp.ViewModels.Physics;
using LaboratoryApp.ViewModels.Chemistry;
using LaboratoryApp.ViewModels.English;


namespace LaboratoryApp.ViewModels.UI
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        private readonly MathsMainWindowViewModel _mathMainWindowVM;
        private readonly PhysicsMainWindowViewModel _physicsMainWindowVM;
        private readonly ChemistryMainWindowViewModel _chemistryMainWindowVM;
        private readonly EnglishMainWindowViewModel _englishMainWindowVM;

        #region commands
        public ICommand NavigateToMathMainWindow { get; set; } // Math
        public ICommand NavigateToPhysicsMainWindow { get; set; } // Physics
        public ICommand NavigateToChemistryMainWindow { get; set; } // Chemistry
        public ICommand NavigateToEnglishMainWindow { get; set; } // English
        #endregion

        public DashboardViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Initialize Sub ViewModels
            _mathMainWindowVM = new MathsMainWindowViewModel(_navigationService);
            _physicsMainWindowVM = new PhysicsMainWindowViewModel(_navigationService);
            _chemistryMainWindowVM = new ChemistryMainWindowViewModel(_navigationService);
            _englishMainWindowVM = new EnglishMainWindowViewModel(_navigationService);

            #region Initialize sub pages with their respective view models
            var mathPage = new MathsMainWindow
            {
                DataContext = new MathsMainWindowViewModel(_navigationService)
            };

            var physicsPage = new PhysicsMainWindow
            {
                DataContext = new PhysicsMainWindowViewModel(_navigationService)
            };

            var chemistryPage = new ChemistryMainWindow
            {
                DataContext = _chemistryMainWindowVM
            };

            var englishPage = new EnglishMainWindow
            {
                DataContext = new EnglishMainWindowViewModel(_navigationService)
            };
            #endregion

            // Navigate to the math page
            NavigateToMathMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(mathPage);
            });

            // Navigate to the physics page
            NavigateToPhysicsMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(physicsPage);
            });

            // Navigate to the chemistry page
            NavigateToChemistryMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(chemistryPage);
            });

            // Navigate to the english page
            NavigateToEnglishMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateTo(englishPage);
            });
        }
    }
}
