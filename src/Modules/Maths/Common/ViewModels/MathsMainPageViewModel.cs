using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Maths.Common.ViewModels
{
    class MathsMainPageViewModel : BaseViewModel
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
