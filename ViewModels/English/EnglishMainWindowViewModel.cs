using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LaboratoryApp.Services;
using LaboratoryApp.Support.Interface;
using LaboratoryApp.ViewModels.English.SubWin;
using LaboratoryApp.Views.English.SubWin;

namespace LaboratoryApp.ViewModels.English
{
    class EnglishMainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        public ICommand OpenDictionaryCommand { get; set; } // Open Dictionary
        public ICommand NavigateToFlashcardCommand { get; set; } // Navigate to Flashcard
        #endregion

        public EnglishMainWindowViewModel(INavigationService navigationService)
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
        }
    }
}
