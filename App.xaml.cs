using System.Configuration;
using System.Data;
using System.Windows;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Data.Providers.Authentication;
using LaboratoryApp.src.Modules.Authentication.Views;
using LaboratoryApp.src.Modules.Authentication.ViewModels;
using LaboratoryApp.src.Services.Authentication;

namespace LaboratoryApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new AuthenticationWindow
            {
                DataContext = new AuthenticationViewModel()
            };
            window.ShowDialog();
        }
    }

}
