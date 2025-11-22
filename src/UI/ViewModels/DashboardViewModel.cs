using LaboratoryApp.src.Core.Interfaces;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.Dashboard.Views;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.Views;
using LaboratoryApp.src.Modules.Tools.English.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.Maths.Dashboard.Views;
using LaboratoryApp.src.Modules.Tools.Physics.Dashboard.Views;
using LaboratoryApp.src.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace LaboratoryApp.src.UI.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand NavigateToMathMainPage { get; set; } // Math
        public ICommand NavigateToPhysicsMainPage { get; set; } // Physics
        public ICommand NavigateToChemistryMainPage { get; set; } // Chemistry
        public ICommand NavigateToEnglishMainPage { get; set; } // English
        public ICommand NavigateToAssignmentMainPage { get; set; } // Assignment
        #endregion

        public DashboardViewModel(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            // Navigate to the math page
            NavigateToMathMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<MathsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the physics page
            NavigateToPhysicsMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<PhysicsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the chemistry page
            NavigateToChemistryMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<ChemistryMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the english page
            NavigateToEnglishMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<EnglishMainPage>();
                _navigationService.NavigateTo(page);
            });

            // 
            NavigateToAssignmentMainPage = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<ExerciseSetManagerPage>();
                if (page.DataContext is ExerciseSetManagerViewModel vm && vm is IAsyncInitializable init)
                {
                    _ = init.InitializeAsync();
                }
                _navigationService.NavigateTo(page);
            });
        }
    }
}
