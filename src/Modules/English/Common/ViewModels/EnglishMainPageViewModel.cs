using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

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
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand OpenDictionaryCommand { get; set; } // Open Dictionary
        public ICommand NavigateToFlashcardManagerCommand { get; set; } // Navigate to Flashcard
        public ICommand NavigateToLectureCommand { get; set; } // Navigate to Lecture
        #endregion

        public EnglishMainPageViewModel(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            // Load all datas from database
            EnglishDataCache.LoadAllData(new EnglishService());

            // Open Dictionary Command
            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open Dictionary Window
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                window.Show();
            });
            NavigateToFlashcardManagerCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<FlashcardManagerPage>();
                _navigationService.NavigateTo(page);
            });
            NavigateToLectureCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Navigate to Lecture Page
                var page = _serviceProvider.GetRequiredService<LectureMainPage>();
                _navigationService.NavigateTo(page);
            });
        }
    }
}
