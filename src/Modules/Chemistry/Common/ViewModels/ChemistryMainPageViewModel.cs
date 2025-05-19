using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Chemistry.Common.ViewModels
{
    public class ChemistryMainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        public ICommand OpenPeriodicTableCommand { get; set; } // Command to open the periodic table window
        public ICommand NavigateToCompoundCommand { get; set; } // Command to navigate to the compound page
        public ICommand NavigateToReactionCommand { get; set; } // Command to navigate to the reaction page
        #endregion

        public ChemistryMainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            ChemistryDataCache.LoadAllData(new ChemistryService());

            var compoundPage = new CompoundManagerPage
            {
                DataContext = new CompoundManagerViewModel()
            };

            var reactionPage = new ReactionManagerPage
            {
                DataContext = new ReactionManagerViewModel()
            };

            // Initialize commands
            OpenPeriodicTableCommand = new RelayCommand<object>((p)=>true, (p) =>
            {
                var periodicWindow = new PeriodicTableWindow
                {
                    DataContext = new PeriodicTableViewModel()
                };
                periodicWindow.Show();
            });

            NavigateToCompoundCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Navigate to the compound page
                _navigationService.NavigateTo(compoundPage);
            });

            NavigateToReactionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Navigate to the reaction page
                _navigationService.NavigateTo(reactionPage);
            });
        }
    }
}
