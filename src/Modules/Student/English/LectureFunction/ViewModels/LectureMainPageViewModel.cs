using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Student.English.LectureFunction.Views;
using LaboratoryApp.src.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Student.English.LectureFunction.ViewModels
{
    public class LectureMainPageViewModel : BaseViewModel
    {
        private readonly INavigateService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand NavigateToContentCommand { get; set; } // Open Lecture
        #endregion

        public LectureMainPageViewModel(INavigateService navigationService,
                                        IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            NavigateToContentCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var lectureContentPage = _serviceProvider.GetRequiredService<LectureContentPage>();
                _navigationService.NavigateTo(lectureContentPage);
            });
        }
    }
}
