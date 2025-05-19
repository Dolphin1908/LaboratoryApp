using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.LectureFunction.Views;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.English.LectureFunction.ViewModels
{
    public class LectureMainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        public ICommand NavigateToContentCommand { get; set; } // Open Lecture
        #endregion

        public LectureMainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateToContentCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var lectureContentPage = new LectureContentPage
                {
                    DataContext = new LectureContentViewModel()
                };
                _navigationService.NavigateTo(lectureContentPage);
            });
        }
    }
}
