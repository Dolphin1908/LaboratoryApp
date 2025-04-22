using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.Support.Interface;
using LaboratoryApp.Views.English.Lecture;

namespace LaboratoryApp.ViewModels.English.Lecture
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
