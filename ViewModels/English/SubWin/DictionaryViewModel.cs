using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Speech.Synthesis;

using LaboratoryApp.Models.English;
using LaboratoryApp.Services;
using LaboratoryApp.Models.DTOs;
using System.Windows.Input;
using System.Windows;

namespace LaboratoryApp.ViewModels.English.SubWin
{
    class DictionaryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string _searchText;

        private WordResultDTO _selectedWordResult;
        private ObservableCollection<SearchResultDTO> _searchResultDTOs;

        private List<WordModel> _allWords;
        private List<PosModel> _allPos;
        private List<ExampleModel> _allExamples;
        private List<DefinitionModel> _allDefinitions;

        #region Commands
        public ICommand SelectResultCommand { get; set; }
        public ICommand SpeechTextCommand { get; set; }
        #endregion

        #region Properties
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                UpdateSuggestions();
            }
        }

        public ObservableCollection<SearchResultDTO> SearchResultDTOs
        {
            get => _searchResultDTOs;
            set
            {
                _searchResultDTOs = value;
                OnPropertyChanged();
            }
        }

        public WordResultDTO SelectedWordResult
        {
            get => _selectedWordResult;
            set
            {
                _selectedWordResult = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public DictionaryViewModel()
        {
            // Initialize commands and properties here if needed
            _searchResultDTOs = new ObservableCollection<SearchResultDTO>();
            SelectedWordResult = null;
            SelectResultCommand = new RelayCommand<SearchResultDTO>((p) => true, (p) => SelectResult(p));
            SpeechTextCommand = new RelayCommand<string>((p) => true, (p) =>
            {
                if (p != null)
                {
                    var synthesizer = new SpeechSynthesizer();
                    synthesizer.Speak(p);
                }
            });
            LoadData();
            SetupSpeechSynthesizer();
        }

        /// <summary>
        /// Updates the suggestions based on the search text.
        /// </summary>
        private void UpdateSuggestions()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResultDTOs.Clear();
                return;
            }

            var matches = _allWords.Where(w => w.headword.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase)) // case insensitive
                                   .Take(10) // limit to 10 results
                                   .ToList();

            SearchResultDTOs.Clear();
            foreach (var match in matches)
            {
                var pos = _allPos.FirstOrDefault(p => p.word_id == match.Id);
                if (pos != null)
                {
                    var def = _allDefinitions.FirstOrDefault(d => d.pos_id == pos.Id);
                    SearchResultDTOs.Add(new SearchResultDTO
                    {
                        WordId = match.Id,
                        Word = match.headword,
                        Pos = pos.pos, // Use pos only if it's not null
                        Meaning = def?.definition ?? "" // If def is null, use empty string
                    });
                }
                else
                {
                    // Handle the case where pos is null, e.g., add a result with default values
                    SearchResultDTOs.Add(new SearchResultDTO
                    {
                        WordId = match.Id,
                        Word = match.headword,
                        Pos = "", // Default value when pos is null
                        Meaning = "" // Default value when definition is null
                    });
                }
            }
        }

        /// <summary>
        /// Handles the selection of a search result.
        /// </summary>
        /// <param name="selected"></param>
        private void SelectResult(SearchResultDTO selected)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a word from the list.");
                return;
            }

            // Handle the selection of a search result
            var selectedWord = _allWords.FirstOrDefault(w => w.Id == selected.WordId);

            // Get all pos for the selected word
            List<PosDTO> posList = new List<PosDTO>();
            foreach (var pos in _allPos.Where(p => p.word_id == selectedWord.Id))
            {
                List<DefinitionDTO> defList = new List<DefinitionDTO>();
                foreach (var def in _allDefinitions.Where(d => d.pos_id == pos.Id))
                {
                    List<ExampleDTO> exampleList = new List<ExampleDTO>();
                    foreach (var ex in _allExamples.Where(e => e.def_id == def.Id))
                    {
                        // Create a new ExampleDTO object for each example
                        var parts = ex.example.Split(new[] { '+' }, 2);
                        exampleList.Add(new ExampleDTO
                        {
                            id = ex.Id,
                            def_id = ex.def_id,
                            example = parts[0].Trim(),
                            translation = parts.Length > 1 ? parts[1].Trim() : "" // Handle the case where there is no translation
                        });
                    }
                    // Add the definition with its examples to the list
                    defList.Add(new DefinitionDTO
                    {
                        id = def.Id,
                        pos_id = def.pos_id,
                        definition = def.definition,
                        examples = exampleList
                    });
                }
                // Add the pos with its definitions to the list
                posList.Add(new PosDTO
                {
                    id = pos.Id,
                    word_id = pos.word_id,
                    pos = pos.pos,
                    definitions = defList
                });
            }
            // Create the WordResultDTO object with the selected word and its pos
            SelectedWordResult = new WordResultDTO
            {
                id = selectedWord.Id,
                word = selectedWord.headword,
                pronunciation = selectedWord.phonetic,
                pos = posList
            };

            SearchText = "";
            SearchResultDTOs.Clear();
        }

        /// <summary>
        /// Loads all data from the database.
        /// </summary>
        private void LoadData()
        {
            _allWords = EnglishDataCache.AllWords;
            _allPos = EnglishDataCache.AllPos;
            _allExamples = EnglishDataCache.AllExamples;
            _allDefinitions = EnglishDataCache.AllDefinitions;
        }

        private void SetupSpeechSynthesizer()
        {
            // Initialize the SpeechSynthesizer
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoice("Microsoft Zira Desktop"); // Select a voice
            synthesizer.Volume = 100; // Set volume (0-100)
            synthesizer.Rate = 0; // Set rate (-10 to 10)
        }
    }
}
