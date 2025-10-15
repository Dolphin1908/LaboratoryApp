using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.English;
using LaboratoryApp.src.Data.Providers.English.FlashcardFunction;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Modules.English.FlashcardFunction.Views;
using LaboratoryApp.src.Services.English.FlashcardFunction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels
{
    public class FlashcardManagerViewModel : BaseViewModel
    {
        // Services
        private readonly IFlashcardService _flashcardService; // Service to manage flashcards
        private readonly IServiceProvider _serviceProvider; // Service to manage flashcard sets

        // Fields
        private FlashcardSet _selectedFlashcardSet; // Currently selected flashcard set
        private ObservableCollection<FlashcardSet> _flashcardSets; // List of flashcard sets
        private string _searchText; // Text for searching flashcards

        // Factories
        private readonly Func<IFlashcardService, FlashcardSet, FlashcardSetViewModel> _flashcardSetVmFactory;
        private readonly Func<IServiceProvider, IFlashcardService, long, Flashcard, FlashcardViewModel> _flashcardVmFactory; // Factory để tạo FlashcardViewModel cho việc thêm/sửa thẻ flashcard
        private readonly Func<FlashcardSet, IFlashcardService, FlashcardStudyViewModel> _flashcardStudyVmFactory; // Factory để tạo FlashcardStudyViewModel cho việc ôn tập thẻ flashcard
        private readonly Func<DictionaryWindow> _dictionaryWindowFactory; // Factory để tạo DictionaryWindow

        #region Commands
        public ICommand OpenUpdateFlashcardSetWindowCommand { get; set; } // Command to open the edit window
        public ICommand OpenFlashcardSetCommand { get; set; } // Command to open a flashcard set
        public ICommand AddFlashcardSetCommand { get; set; } // Command to add a new flashcard set
        public ICommand DeleteFlashcardSetCommand { get; set; } // Command to delete a flashcard set
        public ICommand OpenAddFlashcardWindowCommand { get; set; } // Command to open the add flashcard window
        public ICommand OpenUpdateFlashcardWindowCommand { get; set; } // Command to update a flashcard
        public ICommand DeleteFlashcardCommand { get; set; } // Command to delete a flashcard
        public ICommand StartFlashcardSetCommand { get; set; } // Command to start a flashcard set
        #endregion

        #region Properties
        public FlashcardSet SelectedFlashcardSet
        {
            get => _selectedFlashcardSet;
            set
            {
                _selectedFlashcardSet = value;
                OnPropertyChanged(nameof(SelectedFlashcardSet));
            }
        }

        public ObservableCollection<FlashcardSet> FlashcardSets
        {
            get => _flashcardSets;
            set
            {
                _flashcardSets = value;
                OnPropertyChanged(nameof(FlashcardSets));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                UpdateSuggestions();
            }
        }
        #endregion

        public FlashcardManagerViewModel(IFlashcardService flashcardService,
                                         IServiceProvider serviceProvider,
                                         Func<IFlashcardService, FlashcardSet, FlashcardSetViewModel> flashcardSetVmFactory,
                                         Func<IServiceProvider, IFlashcardService, long, Flashcard, FlashcardViewModel> flashcardVmFactory,
                                         Func<FlashcardSet, IFlashcardService, FlashcardStudyViewModel> flashcardStudyVmFactory,
                                         Func<DictionaryWindow> dictionaryWindowFactory)
        {
            _flashcardService = flashcardService;
            _serviceProvider = serviceProvider;
            _flashcardSetVmFactory = flashcardSetVmFactory;
            _flashcardVmFactory = flashcardVmFactory;
            _flashcardStudyVmFactory = flashcardStudyVmFactory;
            _dictionaryWindowFactory = dictionaryWindowFactory;

            _flashcardSets = new ObservableCollection<FlashcardSet>(_flashcardService.GetAllSets());

            // Initialize commands
            OpenFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => SelectedFlashcardSet = FlashcardSets.FirstOrDefault(set => set.Id == (long)p)); // không dùng RelayCommand<long> được vì không thể cast từ object sang long

            AddFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => AddNewSet());

            OpenUpdateFlashcardSetWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<UpdateFlashcardSetWindow>();
                window.DataContext = _flashcardSetVmFactory(_flashcardService, SelectedFlashcardSet);
                window.ShowDialog();
            });

            DeleteFlashcardSetCommand = new RelayCommand<FlashcardSet>((p) => true, (p) => DeleteSet(p));

            OpenAddFlashcardWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcard = new Flashcard();

                var window = _serviceProvider.GetRequiredService<FlashcardWindow>();
                window.DataContext = _flashcardVmFactory(_serviceProvider, _flashcardService, SelectedFlashcardSet.Id, flashcard);
                window.ShowDialog();
            });

            OpenUpdateFlashcardWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcard = SelectedFlashcardSet.Flashcards.FirstOrDefault(f => f.Id == (long)p);
                if (flashcard == null)
                {
                    MessageBox.Show("Thẻ flashcard không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var window = _serviceProvider.GetRequiredService<FlashcardWindow>();
                window.DataContext = _flashcardVmFactory(_serviceProvider, _flashcardService, SelectedFlashcardSet.Id, flashcard);
                window.ShowDialog();
            });

            DeleteFlashcardCommand = new RelayCommand<object>((p) => true, (p) => DeleteFlashcard((long)p));

            StartFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var dueFlashcards = SelectedFlashcardSet.Flashcards.Where(f => f.NextReview < DateTime.Now).ToList();
                if (dueFlashcards.Count == 0)
                {
                    if(MessageBox.Show($"Bộ thẻ '{SelectedFlashcardSet.Name}' không có thẻ nào cần ôn tập!", "Bạn có muốn tiếp tục học?", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
                        return;
                }

                var window = _serviceProvider.GetRequiredService<FlashcardStudyWindow>();
                window.DataContext = _flashcardStudyVmFactory(SelectedFlashcardSet, _flashcardService);
                window.ShowDialog();
            });
        }

        private void AddNewSet()
        {
            _flashcardService.CreateNewSet("Bộ thẻ mới", "Mô tả...");
        }

        private void DeleteSet(FlashcardSet setToDelete)
        {
            if (MessageBox.Show($"Bạn có chắc muốn xóa {setToDelete.Name}?", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _flashcardService.DeleteSet(setToDelete.Id);
            }
        }

        private void DeleteFlashcard(long cardId)
        {
            _flashcardService.DeleteCardFromSet(_selectedFlashcardSet.Id, cardId);
            OnPropertyChanged(nameof(SelectedFlashcardSet));
        }

        /// <summary>
        /// Update the suggestions based on the search text.
        /// </summary>
        private void UpdateSuggestions()
        {
            if(string.IsNullOrWhiteSpace(SearchText))
            {
                FlashcardSets = new ObservableCollection<FlashcardSet>(_flashcardService.GetAllSets());
                return;
            }    

            var matches = _flashcardSets
                          .Where(set => set.Name.Contains(SearchText,StringComparison.OrdinalIgnoreCase))
                          .ToList();

            FlashcardSets.Clear();
            foreach (var match in matches)
            {
                FlashcardSets.Add(match);
            }
        }
    }
}
