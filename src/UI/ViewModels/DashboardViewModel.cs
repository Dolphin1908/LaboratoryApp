using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.Common.Views;
using LaboratoryApp.src.Modules.Chemistry.Common.ViewModels;
using LaboratoryApp.src.Modules.English.Common.Views;
using LaboratoryApp.src.Modules.English.Common.ViewModels;
using LaboratoryApp.src.Modules.Physics.Common.Views;
using LaboratoryApp.src.Modules.Physics.Common.ViewModels;
using LaboratoryApp.src.Modules.Maths.Common.Views;
using LaboratoryApp.src.Modules.Maths.Common.ViewModels;
using LaboratoryApp.src.Shared.Interface;
using System.Windows.Controls;

namespace LaboratoryApp.src.UI.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand NavigateToMathMainWindow { get; set; } // Math
        public ICommand NavigateToPhysicsMainWindow { get; set; } // Physics
        public ICommand NavigateToChemistryMainWindow { get; set; } // Chemistry
        public ICommand NavigateToEnglishMainWindow { get; set; } // English
        #endregion

        public DashboardViewModel(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            // Navigate to the math page
            NavigateToMathMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<MathsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the physics page
            NavigateToPhysicsMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<PhysicsMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the chemistry page
            NavigateToChemistryMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<ChemistryMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to the english page
            NavigateToEnglishMainWindow = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<EnglishMainPage>();
                _navigationService.NavigateTo(page);
            });
        }
    }
}
