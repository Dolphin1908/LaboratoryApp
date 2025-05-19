using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Physics.Common.ViewModels
{
    class PhysicsMainPageViewModel : BaseViewModel
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
