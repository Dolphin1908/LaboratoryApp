using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Assignment.Common.Views;
using LaboratoryApp.src.Modules.Chemistry.Common.Views;
using LaboratoryApp.src.Modules.English.Common.Views;
using LaboratoryApp.src.Modules.Physics.Common.Views;
using LaboratoryApp.src.Modules.Maths.Common.Views;

using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Modules.Assignment.Common.ViewModels;

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
                var page = _serviceProvider.GetRequiredService<AssignmentMainPage>();
                if (page.DataContext is AssignmentMainPageViewModel vm && vm is IAsyncInitializable init)
                {
                    _ = init.InitializeAsync();
                }
                _navigationService.NavigateTo(page);
            });
        }
    }
}
