using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.FlashcardFunction.Views;
using LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels;
using LaboratoryApp.src.Modules.English.LectureFunction.Views;
using LaboratoryApp.src.Modules.English.LectureFunction.ViewModels;
using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.English.Common.ViewModels
{
    class EnglishMainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        public ICommand OpenDictionaryCommand { get; set; } // Open Dictionary
        public ICommand NavigateToFlashcardManagerCommand { get; set; } // Navigate to Flashcard
        public ICommand NavigateToLectureCommand { get; set; } // Navigate to Lecture
        #endregion

        public EnglishMainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Load all datas from database
            EnglishDataCache.LoadAllData(new EnglishService());

            // Open Dictionary Command
            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open Dictionary Window
                var dictionaryWindow = new DictionaryWindow
                {
                    DataContext = new DictionaryViewModel()
                };
                dictionaryWindow.Show();
            });
            NavigateToFlashcardManagerCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcardManagerPage = new FlashcardManagerPage
                {
                    DataContext = new FlashcardManagerViewModel()
                };
                _navigationService.NavigateTo(flashcardManagerPage);
            });
            NavigateToLectureCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Navigate to Lecture Page
                var lecturePage = new LectureMainPage
                {
                    DataContext = new LectureMainPageViewModel(_navigationService)
                };
                _navigationService.NavigateTo(lecturePage);
            });
        }
    }
}
