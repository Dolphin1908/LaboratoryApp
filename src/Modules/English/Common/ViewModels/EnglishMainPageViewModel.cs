using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.ExerciseFunction.Views;
using LaboratoryApp.src.Modules.English.ExerciseFunction.ViewModels;

using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.English.Common.ViewModels
{
    class EnglishMainPageViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnglishService _englishService;

        #region Commands
        public ICommand OpenDictionaryCommand { get; set; } // Open Dictionary
        public ICommand NavigateToFlashcardManagerCommand { get; set; } // Navigate to Flashcard
        public ICommand NavigateToLectureCommand { get; set; } // Navigate to Lecture
        public ICommand NavigateToDiaryCommand { get; set; } // Navigate to Diary UI
        public ICommand NavigateToPracticeCommand { get; set; } // Navigate to Practice
        #endregion

        public EnglishMainPageViewModel(INavigationService navigationService,
                                        IServiceProvider serviceProvider,
                                        IEnglishService englishService)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _englishService = englishService;

            // Open Dictionary Command
            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open Dictionary Window
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                if (window.DataContext is DictionaryViewModel vm && vm is IAsyncInitializable init)
                {
                    // Initialize the dictionary window asynchronously
                    _ = init.InitializeAsync();
                }
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

            // Navigate to Practice Command
            NavigateToPracticeCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var page = _serviceProvider.GetRequiredService<ExerciseSetManagerPage>();
                if (page.DataContext is ExerciseSetManagerViewModel vm && vm is IAsyncInitializable initPage)
                {
                    // Initialize the practice page asynchronously
                    _ = initPage.InitializeAsync();
                }
                _navigationService.NavigateTo(page);
            });
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Load any additional data or perform setup tasks here
                EnglishDataCache.LoadAllData(_englishService);
            }, cancellationToken);

            Application.Current.Dispatcher.Invoke(() =>
            {
                // Update UI elements if necessary after loading data
                OnPropertyChanged(nameof(NavigateToDiaryCommand));
            });
        }
    }
}
