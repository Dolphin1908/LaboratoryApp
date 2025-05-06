using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using LaboratoryApp.Models.English;
using LaboratoryApp.Services;
using LaboratoryApp.Views.English.Flashcard.SubWin;
using LaboratoryApp.ViewModels.English.Flashcard.SubWin;

namespace LaboratoryApp.ViewModels.English.Flashcard.UI
{
    public class FlashcardManagerViewModel : BaseViewModel
    {
        // Fields
        private FlashcardSet _selectedFlashcardSet; // Currently selected flashcard set
        private ObservableCollection<FlashcardSet> _flashcardSets; // List of flashcard sets
        private DateTime _nearlyNextReview; // Date for the next review
        private bool _canStartSet; // Flag to check if the set can be started
        private string _startSetButtonText; // Text for the start set button
        private string _searchText; // Text for searching flashcards

        // Services
        private FlashcardService _flashcardService; // Service to manage flashcards

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
                if (_selectedFlashcardSet == value) return;

                // Hủy đăng ký sự kiện từ set cũ
                UnsubscribeFromFlashcardEvents(_selectedFlashcardSet);

                _selectedFlashcardSet = value;
                OnPropertyChanged(nameof(SelectedFlashcardSet));

                // Đăng ký sự kiện cho set mới
                SubscribeToFlashcardEvents(_selectedFlashcardSet);

                UpdateNearlyNextReview();
                OnPropertyChanged(nameof(CanStartSet));
                OnPropertyChanged(nameof(StartSetButtonText));
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

        public DateTime NearlyNextReview
        {
            get => _nearlyNextReview;
            set
            {
                _nearlyNextReview = value;
                OnPropertyChanged(nameof(NearlyNextReview));
                OnPropertyChanged(nameof(CanStartSet));
                OnPropertyChanged(nameof(StartSetButtonText));
            }
        }

        public bool CanStartSet
        {
            get
            {
                if (SelectedFlashcardSet != null && SelectedFlashcardSet.Flashcards.Count == 0) 
                    return false;
                if (SelectedFlashcardSet != null && SelectedFlashcardSet.Flashcards.All(f => f.IsLearned)) 
                    return false;
                return NearlyNextReview < DateTime.Now;
            }
        }

        public string StartSetButtonText
        {
            get
            {
                if (CanStartSet) 
                    return "Bắt đầu";
                if (SelectedFlashcardSet != null && SelectedFlashcardSet.Flashcards.Count == 0)
                    return "Không có thẻ nào";
                return $"Đợi tới: {NearlyNextReview:HH:mm dd/MM/yyyy}";
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

        #region OnPropertyChanged
        private void SubscribeToFlashcardEvents(FlashcardSet set)
        {
            if (set == null) return;

            set.Flashcards.CollectionChanged += Flashcards_CollectionChanged;
            foreach (var flashcard in set.Flashcards)
            {
                flashcard.PropertyChanged += Flashcard_PropertyChanged;
            }
        }

        private void UnsubscribeFromFlashcardEvents(FlashcardSet set)
        {
            if (set == null) return;

            set.Flashcards.CollectionChanged -= Flashcards_CollectionChanged;
            foreach (var flashcard in set.Flashcards)
            {
                flashcard.PropertyChanged -= Flashcard_PropertyChanged;
            }
        }

        private void Flashcards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Xử lý khi thêm/xóa flashcard
            if (e.NewItems != null)
            {
                foreach (FlashcardModel item in e.NewItems)
                {
                    item.PropertyChanged += Flashcard_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (FlashcardModel item in e.OldItems)
                {
                    item.PropertyChanged -= Flashcard_PropertyChanged;
                }
            }

            UpdateNearlyNextReview();
            OnPropertyChanged(nameof(CanStartSet));
            OnPropertyChanged(nameof(StartSetButtonText));
        }

        private void Flashcard_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Khi NextReview hoặc IsLearned thay đổi
            if (e.PropertyName == nameof(FlashcardModel.NextReview) ||
                e.PropertyName == nameof(FlashcardModel.IsLearned))
            {
                UpdateNearlyNextReview();
                OnPropertyChanged(nameof(CanStartSet));
                OnPropertyChanged(nameof(StartSetButtonText));
            }
        }

        private void UpdateNearlyNextReview()
        {
            if (SelectedFlashcardSet == null || !SelectedFlashcardSet.Flashcards.Any())
            {
                NearlyNextReview = DateTime.MaxValue;
                return;
            }

            NearlyNextReview = SelectedFlashcardSet.Flashcards.Any()
                ? SelectedFlashcardSet.Flashcards.Min(f => f.NextReview)
                : DateTime.Now;

            OnPropertyChanged(nameof(CanStartSet));
        }
        #endregion

        public FlashcardManagerViewModel()
        {
            // Initialize commands here if needed
            _flashcardSets = new ObservableCollection<FlashcardSet>();
            _flashcardService = new FlashcardService();
            _selectedFlashcardSet = null;
            LoadFlashcardSets();

            // Initialize commands
            OpenFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => OpenFlashcardSet((long)p)); // không dùng RelayCommand<long> được vì không thể cast từ object sang long
            AddFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => AddNewSet());
            OpenUpdateFlashcardSetWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new UpdateFlashcardSetWindow
                {
                    DataContext = new FlashcardViewModel(SelectedFlashcardSet, UpdateSet)
                };
                window.ShowDialog();
            });
            DeleteFlashcardSetCommand = new RelayCommand<FlashcardSet>((p) => true, (p) => DeleteSet(p));
            OpenAddFlashcardWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcard = new FlashcardModel();

                var window = new AddFlashcardWindow
                {
                    DataContext = new FlashcardViewModel(flashcard, AddNewFlashcard)
                };
                window.ShowDialog();
            });
            OpenUpdateFlashcardWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcard = _selectedFlashcardSet.Flashcards.FirstOrDefault(f => f.Id == (long)p);

                var window = new UpdateFlashcardWindow
                {
                    DataContext = new FlashcardViewModel(flashcard, UpdateFlashcard)
                };
                window.ShowDialog();
            });
            DeleteFlashcardCommand = new RelayCommand<object>((p) => true, (p) => DeleteFlashcard((long)p));
            StartFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new FlashcardStudyWindow
                {
                    DataContext = new FlashcardStudyViewModel(SelectedFlashcardSet, _flashcardService)
                };
                window.ShowDialog();
            });
        }

        /// <summary>
        /// Open a flashcard set detail by its ID.
        /// </summary>
        /// <param name="id"></param>
        private void OpenFlashcardSet(long id)
        {
            SelectedFlashcardSet = FlashcardSets.FirstOrDefault(set => set.Id == id);
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
                Flashcards = new ObservableCollection<FlashcardModel>()
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
            var index = FlashcardSets.IndexOf(deletedSet);
            if (index != -1)
            {
                FlashcardSets.RemoveAt(index); // Remove the set from the list
            }
        }

        /// <summary>
        /// Add a new flashcard to the selected set.
        /// </summary>
        /// <param name="flashcard"></param>
        private void AddNewFlashcard(FlashcardModel flashcard)
        {
            _flashcardService.AddFlashcardToSet(_selectedFlashcardSet.Id, flashcard); // Add the new flashcard to the selected set
            OnPropertyChanged(nameof(SelectedFlashcardSet));
        }

        /// <summary>
        /// Update the selected flashcard with new information.
        /// </summary>
        /// <param name="flashcard"></param>
        private void UpdateFlashcard(FlashcardModel flashcard)
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

            var matches = _flashcardSets.Where(set => set.Name.Contains(SearchText,StringComparison.OrdinalIgnoreCase)).ToList();

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
