using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels;

namespace LaboratoryApp.src.Modules.Chemistry.Common.ViewModels
{
    public class ChemistryMainPageViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IChemistryService _chemistryService;
        private readonly ChemistryDataCache _chemistryDataCache;

        #region Commands
        public ICommand OpenPeriodicTableCommand { get; set; } // Command to open the periodic table window
        public ICommand NavigateToCompoundCommand { get; set; } // Command to navigate to the compound page
        public ICommand NavigateToReactionCommand { get; set; } // Command to navigate to the reaction page
        #endregion

        /// <summary>
        /// ViewModel for the main chemistry page, providing commands to navigate to different chemistry functionalities.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="chemistryService"></param>
        /// <param name="chemistryDataCache"></param>
        public ChemistryMainPageViewModel(INavigationService navigationService, 
                                          IServiceProvider serviceProvider,
                                          IChemistryService chemistryService,
                                          ChemistryDataCache chemistryDataCache)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _chemistryService = chemistryService;
            _chemistryDataCache = chemistryDataCache;

            #region Commands
            OpenPeriodicTableCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<PeriodicTableWindow>();
                window.Show();
                if (window.DataContext is IAsyncInitializable init)
                {
                    // Initialize the periodic table window asynchronously
                    _ = init.InitializeAsync();
                }
            });

            NavigateToCompoundCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<CompoundManagerPage>();
                _navigationService.NavigateTo(page);
                if(page.DataContext is CompoundManagerViewModel vm && vm is IAsyncInitializable initPage)
                {
                    // Initialize the compound manager page asynchronously
                    _ = initPage.InitializeAsync();
                }
            });

            NavigateToReactionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Navigate to the reaction page
                var page = _serviceProvider.GetRequiredService<ReactionManagerPage>();
                _navigationService.NavigateTo(page);
                if (page.DataContext is ReactionManagerViewModel vm && vm is IAsyncInitializable initPage)
                {
                    // Initialize the reaction manager page asynchronously
                    _ = initPage.InitializeAsync();
                }
            });
            #endregion
        }

        /// <summary>
        /// Asynchronously initializes the ChemistryMainPageViewModel, loading necessary data and updating the UI.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Load any additional data or perform setup tasks here
                _chemistryDataCache.LoadAllData(_chemistryService);
            }, cancellationToken);

            Application.Current.Dispatcher.Invoke(() =>
            {
                // Ensure the UI is updated on the main thread
                OnPropertyChanged(nameof(OpenPeriodicTableCommand));
                OnPropertyChanged(nameof(NavigateToCompoundCommand));
                OnPropertyChanged(nameof(NavigateToReactionCommand));
            });
        }
    }
}
