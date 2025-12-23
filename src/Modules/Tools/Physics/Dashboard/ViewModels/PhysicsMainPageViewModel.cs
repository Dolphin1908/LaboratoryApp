using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Interfaces.Services;

namespace LaboratoryApp.src.Modules.Tools.Physics.Dashboard.ViewModels
{
    public class PhysicsMainPageViewModel : BaseViewModel
    {
        private readonly INavigateService _navigationService;

        #region Commands
        #endregion

        public PhysicsMainPageViewModel(INavigateService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
