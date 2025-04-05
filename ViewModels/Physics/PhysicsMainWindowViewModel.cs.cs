using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.Support.Interface;

namespace LaboratoryApp.ViewModels.Physics
{
    class PhysicsMainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        #endregion

        public PhysicsMainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
