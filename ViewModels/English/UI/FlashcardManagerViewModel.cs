using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Input;

using LaboratoryApp.Models.English;
using LaboratoryApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using LaboratoryApp.Views.English.SubWin;
using LaboratoryApp.ViewModels.English.SubWin;

namespace LaboratoryApp.ViewModels.English.UI
{
    public class FlashcardManagerViewModel : BaseViewModel
    {
        // Fields
        private FlashcardSet _selectedFlashcardSet; // Currently selected flashcard set
        private ObservableCollection<FlashcardSet> _flashcardSets; // List of flashcard sets

        // Data
        private List<WordModel> _allWords; // List of all words
        private List<PosModel> _allPos; // List of all parts of speech
        private List<DefinitionModel> _allDefinitions; // List of all definitions
        private List<ExampleModel> _allExamples; // List of all examples

        // Services
        private FlashcardService _flashcardService; // Service to manage flashcards

        #region Commands
        public ICommand OpenUpdateFlashcardSetWindowCommand { get; set; } // Command to open the edit window
        public ICommand OpenFlashcardSetCommand { get; set; } // Command to open a flashcard set
        public ICommand AddFlashcardSetCommand { get; set; } // Command to add a new flashcard set
        public ICommand DeleteFlashcardSetCommand { get; set; } // Command to delete a flashcard set
        public ICommand OpenAddFlashcardWindowCommand { get; set; } // Command to open the add flashcard window
        public ICommand UpdateFlashcardCommand { get; set; } // Command to update a flashcard
        public ICommand DeleteFlashcardCommand { get; set; } // Command to delete a flashcard
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
        #endregion

        public FlashcardManagerViewModel()
        {
            // Initialize commands here if needed
            _flashcardSets = new ObservableCollection<FlashcardSet>();
            _flashcardService = new FlashcardService();
            _selectedFlashcardSet = null;
            LoadWordData();
            LoadFlashcardSets();

            // Initialize commands
            OpenFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => OpenFlashcardSet((long)p)); // không dùng RelayCommand<long> được vì không thể cast từ object sang long
            AddFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => AddNewSet());
            OpenUpdateFlashcardSetWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new UpdateFlashcardSetWindow
                {
                    DataContext = new UpdateFlashcardSetViewModel(SelectedFlashcardSet, UpdateSet)
                };
                window.ShowDialog();
            });
            DeleteFlashcardSetCommand = new RelayCommand<FlashcardSet>((p) => true, (p) => DeleteSet(p));
            OpenAddFlashcardWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new AddFlashcardWindow
                {
                    DataContext = new AddFlashcardViewModel(AddNewFlashcard)
                };
                window.ShowDialog();
            });
            UpdateFlashcardCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var flashcard = _selectedFlashcardSet.flashcards.FirstOrDefault(f => f.id == (long)p);

                var window = new EditFlashcardWindow
                {
                    DataContext = new EditFlashcardViewModel(flashcard, UpdateFlashcard)
                };
                window.ShowDialog();
            });
            DeleteFlashcardCommand = new RelayCommand<object>((p) => true, (p) => DeleteFlashcard((long)p));
        }

        private void OpenFlashcardSet(long id)
        {
            SelectedFlashcardSet = FlashcardSets.FirstOrDefault(set => set.id == id);
        }

        /// <summary>
        /// Add a new flashcard set with default name.
        /// </summary>
        private void AddNewSet()
        {
            // Add new set
            FlashcardSet flashcardSet = new FlashcardSet
            {
                name = "New Set",
                count = 0,
                createdDate = DateTime.Now,
                lastUpdatedDate = DateTime.Now,
                flashcards = new ObservableCollection<FlashcardModel>()
            };
            _flashcardService.AddFlashcardSet(flashcardSet); // Add the new set to the service
            FlashcardSets.Add(flashcardSet); // Add the new set to the list
        }

        private void UpdateSet(FlashcardSet updatedSet)
        {
            updatedSet.lastUpdatedDate = DateTime.Now; // Update the last updated date
            _flashcardService.UpdateFlashcardSet(updatedSet); // Update the set in the service
            var index = FlashcardSets.IndexOf(updatedSet);
            if (index != -1)
            {
                FlashcardSets[index] = updatedSet;
            }
        }

        private void DeleteSet(FlashcardSet deletedSet)
        {
            _flashcardService.DeleteFlashcardSet(deletedSet); // Delete the set from the service
            SelectedFlashcardSet = null;
            var index = FlashcardSets.IndexOf(deletedSet);
            if (index != -1)
            {
                FlashcardSets.RemoveAt(index); // Remove the set from the list
            }
        }

        private void AddNewFlashcard(FlashcardModel flashcard)
        {
            _flashcardService.AddFlashcardToSet(_selectedFlashcardSet.id, flashcard); // Add the new flashcard to the selected set
        }

        private void UpdateFlashcard(FlashcardModel flashcard)
        {
            _flashcardService.UpdateFlashcard(_selectedFlashcardSet.id, flashcard); // Update the flashcard in the selected set
        }

        private void DeleteFlashcard(long flashcardId)
        {
            var flashcard = _selectedFlashcardSet.flashcards.FirstOrDefault(f => f.id == flashcardId);
            _flashcardService.DeleteFlashcardFromSet(_selectedFlashcardSet.id, flashcard); // Delete the flashcard from the selected set
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


        /// <summary>
        /// Load all word data from the database.
        /// </summary>
        private void LoadWordData()
        {
            _allWords = EnglishDataCache.AllWords;
            _allPos = EnglishDataCache.AllPos;
            _allDefinitions = EnglishDataCache.AllDefinitions;
            _allExamples = EnglishDataCache.AllExamples;
        }
    }
}
