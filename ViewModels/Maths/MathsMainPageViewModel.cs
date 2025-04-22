using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.Support.Interface;

namespace LaboratoryApp.ViewModels.Maths
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
