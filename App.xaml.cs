using LaboratoryApp.src.Configuration;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.UI.ViewModels;
using LaboratoryApp.src.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LaboratoryApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Construction
        /// </summary>
        public App()
        {
            var services = new ServiceCollection();

            services.AddApplicationService();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Lấy MainWindowViewModel từ ServiceProvider
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            var navigationService = _serviceProvider.GetRequiredService<INavigateService>();
            var mainWindowVM = _serviceProvider.GetRequiredService<MainWindowViewModel>();

            mainWindow.DataContext = mainWindowVM;
            navigationService.Initialize(mainWindow.MainFrame);

            // Hiển thị MainWindow
            mainWindow.Show();

            if (mainWindowVM is IAsyncInitializable asyncVm)
            {
                await asyncVm.InitializeAsync();
            }
            else
            {
                mainWindowVM.Initialize();
            }

            await AppBootstrapper.InitializeAppDataAsync(_serviceProvider);
        }
    }

}
