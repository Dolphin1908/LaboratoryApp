using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.Services;
using LaboratoryApp.Support.Interface;
using LaboratoryApp.ViewModels.English.DictionaryFunction;
using LaboratoryApp.ViewModels.English.DictionaryFunction.SubWin;
using LaboratoryApp.ViewModels.English.FlashcardFunction.UI;
using LaboratoryApp.ViewModels.English.LectureFunction;
using LaboratoryApp.Views.English.DictionaryFunction.SubWin;
using LaboratoryApp.Views.English.FlashcardFunction.UI;
using LaboratoryApp.Views.English.LectureFunction;

namespace LaboratoryApp.ViewModels.English
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
                var flashcardManagerPage = new FlashcardManager
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
