using LaboratoryApp.Support.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace LaboratoryApp.Support
{
    public class NavigateService : INavigationService
    {
        private readonly NavigationService _navigationService;

        public NavigateService(NavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void NavigateTo(string uri)
        {
            _navigationService.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
        }
    }
}
