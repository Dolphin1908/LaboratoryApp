using LaboratoryApp.Support.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

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

        private readonly MathsMainPageViewModel _mathMainWindowVM;
        private readonly PhysicsMainPageViewModel _physicsMainWindowVM;
        private readonly ChemistryMainPageViewModel _chemistryMainWindowVM;
        private readonly EnglishMainPageViewModel _englishMainWindowVM;

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
            _mathMainWindowVM = new MathsMainPageViewModel(_navigationService);
            _physicsMainWindowVM = new PhysicsMainPageViewModel(_navigationService);
            _chemistryMainWindowVM = new ChemistryMainPageViewModel(_navigationService);
            _englishMainWindowVM = new EnglishMainPageViewModel(_navigationService);

            #region Initialize sub pages with their respective view models
            var mathPage = new MathsMainPage
            {
                DataContext = _mathMainWindowVM
            };

            var physicsPage = new PhysicsMainPage
            {
                DataContext = _physicsMainWindowVM
            };

            var chemistryPage = new ChemistryMainPage
            {
                DataContext = _chemistryMainWindowVM
            };

            var englishPage = new EnglishMainPage
            {
                DataContext = _englishMainWindowVM
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
