using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.Support.Interface;
using LaboratoryApp.ViewModels.Chemistry.PeriodicTable.SubWin;
using LaboratoryApp.Views.Chemistry.PeriodicTable.SubWin;

namespace LaboratoryApp.ViewModels.Chemistry
{
    class ChemistryMainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        #region Commands
        public ICommand NavigateToPeriodicTableCommand { get; set; } // Command to navigate to the periodic table
        #endregion

        public ChemistryMainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Initialize commands
            NavigateToPeriodicTableCommand = new RelayCommand<object>((p)=>true, (p) =>
            {
                var periodicWindow = new PeriodicTableWindow
                {
                    DataContext = new PeriodicTableViewModel()
                };
                periodicWindow.Show();
            });
        }
    }
}
