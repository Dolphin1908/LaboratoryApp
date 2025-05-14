using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LaboratoryApp.Services;
using LaboratoryApp.Support.Interface;
using LaboratoryApp.ViewModels.Chemistry.CompoundFunction.UI;
using LaboratoryApp.ViewModels.Chemistry.PeriodicTableFunction.SubWin;
using LaboratoryApp.Views.Chemistry.CompoundFunction.UI;
using LaboratoryApp.Views.Chemistry.PeriodicTableFunction.SubWin;

namespace LaboratoryApp.ViewModels.Chemistry
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

            var compoundPage = new Compound
            {
                DataContext = new CompoundManagerViewModel()
            };

            var reactionPage = new Reaction
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
