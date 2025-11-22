using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interfaces;

namespace LaboratoryApp.src.Modules.Tools.Physics.Dashboard.ViewModels
{
    public class PhysicsMainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        #endregion

        public PhysicsMainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
