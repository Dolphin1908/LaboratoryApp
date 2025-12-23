using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Interfaces.Services;

namespace LaboratoryApp.src.Modules.Tools.Maths.Dashboard.ViewModels
{
    public class MathsMainPageViewModel : BaseViewModel
    {
        private readonly INavigateService _navigationService;

        #region Commands
        #endregion

        public MathsMainPageViewModel(INavigateService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
