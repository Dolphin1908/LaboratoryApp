using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Chemistry.Common.ViewModels
{
    public class ChemistryMainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand OpenPeriodicTableCommand { get; set; } // Command to open the periodic table window
        public ICommand NavigateToCompoundCommand { get; set; } // Command to navigate to the compound page
        public ICommand NavigateToReactionCommand { get; set; } // Command to navigate to the reaction page
        #endregion

        public ChemistryMainPageViewModel(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            ChemistryDataCache.LoadAllData(new ChemistryService());

            // Initialize commands
            OpenPeriodicTableCommand = new RelayCommand<object>((p)=>true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<PeriodicTableWindow>();
                window.Show();
            });

            NavigateToCompoundCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<CompoundManagerPage>();
                _navigationService.NavigateTo(page);
            });

            NavigateToReactionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Navigate to the reaction page
                var page = _serviceProvider.GetRequiredService<ReactionManagerPage>();
                _navigationService.NavigateTo(page);
            });
        }
    }
}
