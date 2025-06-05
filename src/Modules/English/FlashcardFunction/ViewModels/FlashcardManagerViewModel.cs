using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using LaboratoryApp.src.Modules.English.FlashcardFunction.Views;
using LaboratoryApp.src.Services.English.FlashcardFunction;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;

namespace LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels
{
    public class FlashcardManagerViewModel : BaseViewModel
    {
        // Fields
        private FlashcardSet _selectedFlashcardSet; // Currently selected flashcard set
        private ObservableCollection<FlashcardSet> _flashcardSets; // List of flashcard sets
        private string _searchText; // Text for searching flashcards

        // Services
        private readonly IFlashcardService _flashcardService; // Service to manage flashcards
        private readonly IServiceProvider _serviceProvider; // Service to manage flashcard sets

        // Factories
        private readonly Func<FlashcardSet, Action<FlashcardSet>, FlashcardViewModel> _updateSetVmFactory; // Factory để tạo FlashcardViewModel cho việc cập nhật bộ thẻ
        private readonly Func<Flashcard, Action<Flashcard>, Func<DictionaryWindow>, FlashcardViewModel> _flashcardVmFactory; // Factory để tạo FlashcardViewModel cho việc thêm/sửa thẻ flashcard
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
                                         Func<FlashcardSet, Action<FlashcardSet>, FlashcardViewModel> updateSetVmFactory,
                                         Func<Flashcard, Action<Flashcard>, Func<DictionaryWindow>, FlashcardViewModel> flashcardVmFactory,
                                         Func<FlashcardSet, IFlashcardService, FlashcardStudyViewModel> flashcardStudyVmFactory,
                                         Func<DictionaryWindow> dictionaryWindowFactory)
        {
            _flashcardService = flashcardService;
            _serviceProvider = serviceProvider;
            _updateSetVmFactory = updateSetVmFactory;
            _flashcardVmFactory = flashcardVmFactory;
            _flashcardStudyVmFactory = flashcardStudyVmFactory;
            _dictionaryWindowFactory = dictionaryWindowFactory;

            _flashcardSets = new ObservableCollection<FlashcardSet>();
            LoadFlashcardSets();

            // Initialize commands
            OpenFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => SelectedFlashcardSet = FlashcardSets.FirstOrDefault(set => set.Id == (long)p)); // không dùng RelayCommand<long> được vì không thể cast từ object sang long

            AddFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => AddNewSet());

            OpenUpdateFlashcardSetWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<UpdateFlashcardSetWindow>();
                window.DataContext = _updateSetVmFactory(SelectedFlashcardSet, UpdateSet);
                window.ShowDialog();
            });

            DeleteFlashcardSetCommand = new RelayCommand<FlashcardSet>((p) => true, (p) => DeleteSet(p));

            OpenAddFlashcardWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcard = new Flashcard();

                var window = _serviceProvider.GetRequiredService<AddFlashcardWindow>();
                window.DataContext = _flashcardVmFactory(flashcard, AddNewFlashcard, _dictionaryWindowFactory);
                window.ShowDialog();
            });

            OpenUpdateFlashcardWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcard = _selectedFlashcardSet.Flashcards.FirstOrDefault(f => f.Id == (long)p);
                if (flashcard == null)
                {
                    MessageBox.Show("Thẻ flashcard không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var window = _serviceProvider.GetRequiredService<UpdateFlashcardWindow>();
                window.DataContext = _flashcardVmFactory(flashcard, UpdateFlashcard, _dictionaryWindowFactory);
                window.ShowDialog();
            });

            DeleteFlashcardCommand = new RelayCommand<object>((p) => true, (p) => DeleteFlashcard((long)p));

            StartFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var dueFlashcards = SelectedFlashcardSet.Flashcards
                                                        .Where(f => f.NextReview < DateTime.Now)
                                                        .ToList();
                if (dueFlashcards.Count == 0)
                {
                    if (MessageBoxResult.No == MessageBox.Show($"Bộ thẻ '{SelectedFlashcardSet.Name}' không có thẻ nào cần ôn tập!", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                        return;
                }

                var window = _serviceProvider.GetRequiredService<FlashcardStudyWindow>();
                window.DataContext = _flashcardStudyVmFactory(SelectedFlashcardSet, _flashcardService);
                window.ShowDialog();
            });
        }

        /// <summary>
        /// Add a new flashcard set with default name.
        /// </summary>
        private void AddNewSet()
        {
            // Add new set
            FlashcardSet flashcardSet = new FlashcardSet
            {
                Name = "New Set",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                Flashcards = new ObservableCollection<Flashcard>()
            };
            _flashcardService.AddFlashcardSet(flashcardSet); // Add the new set to the service
            FlashcardSets.Add(flashcardSet); // Add the new set to the list
        }

        /// <summary>
        /// Update the selected flashcard set with new information.
        /// </summary>
        /// <param name="updatedSet"></param>
        private void UpdateSet(FlashcardSet updatedSet)
        {
            updatedSet.LastUpdatedDate = DateTime.Now; // Update the last updated date
            _flashcardService.UpdateFlashcardSet(updatedSet); // Update the set in the service
            var index = FlashcardSets.IndexOf(updatedSet);
            if (index != -1)
            {
                FlashcardSets[index] = updatedSet;
                OnPropertyChanged(nameof(FlashcardSets));
            }
        }

        /// <summary>
        /// Delete the selected flashcard set.
        /// </summary>
        /// <param name="deletedSet"></param>
        private void DeleteSet(FlashcardSet deletedSet)
        {
            // Confirm deletion
            if (MessageBoxResult.No == MessageBox.Show($"Bạn có chắc chắn muốn xóa bộ thẻ '{deletedSet.Name}' không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                return;

            _flashcardService.DeleteFlashcardSet(deletedSet); // Delete the set from the service
            SelectedFlashcardSet = null;
            FlashcardSets.Remove(deletedSet); // Remove the set from the list
        }

        /// <summary>
        /// Add a new flashcard to the selected set.
        /// </summary>
        /// <param name="flashcard"></param>
        private void AddNewFlashcard(Flashcard flashcard)
        {
            _flashcardService.AddFlashcardToSet(_selectedFlashcardSet.Id, flashcard); // Add the new flashcard to the selected set
            OnPropertyChanged(nameof(SelectedFlashcardSet));
        }

        /// <summary>
        /// Update the selected flashcard with new information.
        /// </summary>
        /// <param name="flashcard"></param>
        private void UpdateFlashcard(Flashcard flashcard)
        {
            _flashcardService.UpdateFlashcard(_selectedFlashcardSet.Id, flashcard); // Update the flashcard in the selected set
            OnPropertyChanged(nameof(SelectedFlashcardSet));
        }

        /// <summary>
        /// Delete the selected flashcard from the set.
        /// </summary>
        /// <param name="flashcardId"></param>
        private void DeleteFlashcard(long flashcardId)
        {
            var flashcard = _selectedFlashcardSet.Flashcards.FirstOrDefault(f => f.Id == flashcardId); // Find the flashcard to delete
            if (flashcard == null)
            {
                MessageBox.Show("Thẻ flashcard không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Confirm deletion
            if (MessageBoxResult.No == MessageBox.Show($"Bạn có chắc chắn muốn xóa thẻ '{_selectedFlashcardSet.Flashcards.FirstOrDefault(f => f.Id == flashcardId)?.Word}' không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                return;

            _flashcardService.DeleteFlashcardFromSet(_selectedFlashcardSet.Id, flashcard); // Delete the flashcard from the selected set
            OnPropertyChanged(nameof(SelectedFlashcardSet));
        }

        private void UpdateSuggestions()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadFlashcardSets(); // Load all flashcard sets if search text is empty
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

        /// <summary>
        /// Load flashcard sets from the JSON file.
        /// </summary>
        private void LoadFlashcardSets()
        {
            FlashcardSets.Clear();
            foreach (var set in _flashcardService.GetAllFlashcardSets())
            {
                FlashcardSets.Add(set);
            }
        }
    }
}
