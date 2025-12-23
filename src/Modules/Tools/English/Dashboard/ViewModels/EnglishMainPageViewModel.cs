using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Modules.Tools.English.DiaryFunction.ViewModels;
using LaboratoryApp.src.Modules.Tools.English.DiaryFunction.Views;
using LaboratoryApp.src.Modules.Student.English.FlashcardFunction.Views;
using LaboratoryApp.src.Modules.Student.English.LectureFunction.Views;
using LaboratoryApp.src.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.English.Dashboard.ViewModels
{
    class EnglishMainPageViewModel : BaseViewModel
    {
        private readonly INavigateService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand OpenDictionaryCommand { get; set; } // Open Dictionary
        public ICommand NavigateToFlashcardManagerCommand { get; set; } // Navigate to Flashcard
        public ICommand NavigateToLectureCommand { get; set; } // Navigate to Lecture
        public ICommand NavigateToDiaryCommand { get; set; } // Navigate to Diary UI
        #endregion

        public EnglishMainPageViewModel(INavigateService navigationService,
                                        IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            // Open Dictionary Command
            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open Dictionary Window
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                window.Show();
            });

            // Navigate to Flashcard Manager Command
            NavigateToFlashcardManagerCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<FlashcardManagerPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to Lecture Command
            NavigateToLectureCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Navigate to Lecture Page
                var page = _serviceProvider.GetRequiredService<LectureMainPage>();
                _navigationService.NavigateTo(page);
            });

            // Navigate to Diary Command
            NavigateToDiaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<DiaryManagerPage>();
                if (page.DataContext is DiaryManagerViewModel vm && vm is IAsyncInitializable initPage)
                {
                    // Initialize the diary page asynchronously
                    _ = initPage.InitializeAsync();
                }
                _navigationService.NavigateTo(page);
            });
        }
    }
}
