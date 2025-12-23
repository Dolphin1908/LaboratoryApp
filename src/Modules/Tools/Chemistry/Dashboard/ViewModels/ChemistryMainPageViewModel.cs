using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.ReactionFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.ReactionFunction.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.Chemistry.Dashboard.ViewModels
{
    public class ChemistryMainPageViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly INavigateService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand OpenPeriodicTableCommand { get; set; } // Command to open the periodic table window
        public ICommand NavigateToCompoundCommand { get; set; } // Command to navigate to the compound page
        public ICommand NavigateToReactionCommand { get; set; } // Command to navigate to the reaction page
        public ICommand NavigateBackCommand { get; set; } // Command to navigate back
        #endregion

        #region Properties
        #endregion

        /// <summary>
        /// ViewModel for the main chemistry page, providing commands to navigate to different chemistry functionalities.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="chemistryService"></param>
        /// <param name="chemistryDataCache"></param>
        public ChemistryMainPageViewModel(INavigateService navigationService,
                                          IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

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
                if (page.DataContext is CompoundManagerViewModel vm && vm is IAsyncInitializable initPage)
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

            NavigateBackCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateBack();
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
