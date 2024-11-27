using LaboratoryApp.Support.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace LaboratoryApp.ViewModels.UI
{
    public class DashboardViewModel : BaseViewModel
    {
        #region commands
        public ICommand MoreInfoCommand { get; set; }
        #endregion

        private readonly INavigationService _navigationService;

        public DashboardViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            MoreInfoCommand = new RelayCommand<object>((p) => true, (p) => NavigateToChemInfo());
        }

        private void NavigateToChemInfo()
        {
            _navigationService.NavigateTo("../../Views/UI/ChemInfo.xaml");
        }
    }
}
