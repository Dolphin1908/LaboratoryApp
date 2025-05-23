﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using LaboratoryApp.src.Modules.English.FlashcardFunction.Views;
using LaboratoryApp.src.Services.English.FlashcardFunction;

namespace LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels
{
    public class FlashcardManagerViewModel : BaseViewModel
    {
        // Fields
        private FlashcardSet _selectedFlashcardSet; // Currently selected flashcard set
        private ObservableCollection<FlashcardSet> _flashcardSets; // List of flashcard sets
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
                var flashcard = new Flashcard();

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
                var tempFlashcards = SelectedFlashcardSet.Flashcards.Where(i => i.NextReview < DateTime.Now).ToList();
                if(tempFlashcards.Count == 0)
                {
                    if(MessageBoxResult.No == MessageBox.Show($"Bộ thẻ '{SelectedFlashcardSet.Name}' không có thẻ nào cần ôn tập!", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                        return;
                }

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
