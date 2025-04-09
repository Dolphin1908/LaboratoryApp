using LaboratoryApp.Models.English;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Input;

namespace LaboratoryApp.ViewModels.English.UI
{
    public class FlashcardManagerViewModel : BaseViewModel
    {
        private List<FlashcardSet> _flashcardSets; // List of flashcard sets

        private List<WordModel> _allWords; // List of all words
        private List<PosModel> _allPos; // List of all parts of speech
        private List<DefinitionModel> _allDefinitions; // List of all definitions
        private List<ExampleModel> _allExamples; // List of all examples

        #region Commands
        public ICommand OpenEditWindowCommand { get; set; } // Command to open the edit window
        public ICommand OpenAddWindowCommand { get; set; } // Command to open the add window
        public ICommand OpenFlashcardSetCommand { get; set; } // Command to open a flashcard set
        public ICommand DeleteSetCommand { get; set; } // Command to delete a flashcard set
        public ICommand DeleteFlashcardCommand { get; set; } // Command to delete a flashcard
        public ICommand AddFlashcardCommand { get; set; } // Command to add a flashcard
        #endregion

        public FlashcardManagerViewModel()
        {
            // Initialize commands here if needed
        }

        private void LoadFlashcardSets()
        {
            // Load flashcard sets from the database or any other source
            // For example, you can use a service to fetch the data
            // _flashcardSets = FlashcardService.GetFlashcardSets();
        }

        private void LoadWordData()
        {
            _allWords = EnglishDataCache.AllWords;
            _allPos = EnglishDataCache.AllPos;
            _allDefinitions = EnglishDataCache.AllDefinitions;
            _allExamples = EnglishDataCache.AllExamples;
        }
    }
}
