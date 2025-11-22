using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interfaces;

namespace LaboratoryApp.src.Modules.Tools.Maths.Dashboard.ViewModels
{
    public class MathsMainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        #endregion

        public MathsMainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
