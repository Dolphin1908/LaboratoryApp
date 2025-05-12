using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Synthesis;
using System.Windows.Input;
using System.Windows;

using LaboratoryApp.Models.English.DictionaryFunction;
using LaboratoryApp.Services;
using LaboratoryApp.Models.English.DictionaryFunction.DTOs;

namespace LaboratoryApp.ViewModels.English.DictionaryFunction.SubWin
{
    class DictionaryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string _searchText;
        private string _aiResultMessage;
        private bool _isLoadingAI;
        private WordResultDTO _aiResult;

        private WordResultDTO _selectedWordResult;
        private ObservableCollection<DictionarySearchResultDTO> _dictionarySearchResultDTOs;

        private List<Word> _allWords;
        private List<Pos> _allPos;
        private List<Example> _allExamples;
        private List<Definition> _allDefinitions;

        private Dictionary<long, List<Pos>> _posByWordId;
        private Dictionary<long, List<Definition>> _definitionsByPosId;
        private Dictionary<long, List<Example>> _examplesByDefId;

        private SpeechSynthesizer _synthesizer;
        private AIService _aiService;

        #region Commands
        public ICommand SelectResultCommand { get; set; }
        public ICommand SpeechTextCommand { get; set; }
        public ICommand SearchWithAICommand { get; set; }
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

        public ObservableCollection<DictionarySearchResultDTO> DictionarySearchResultDTOs
        {
            get => _dictionarySearchResultDTOs;
            set
            {
                _dictionarySearchResultDTOs = value;
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

        public WordResultDTO AIResult
        {
            get => _aiResult;
            set
            {
                _aiResult = value;
                OnPropertyChanged();
            }
        }
        public string AIResultMessage
        {
            get => _aiResultMessage;
            set
            {
                _aiResultMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoadingAI
        {
            get => _isLoadingAI;
            set
            {
                _isLoadingAI = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public DictionaryViewModel()
        {
            // Initialize commands and properties here if needed
            _dictionarySearchResultDTOs = new ObservableCollection<DictionarySearchResultDTO>();
            _aiService = new AIService();

            SelectResultCommand = new RelayCommand<DictionarySearchResultDTO>((p) => p != null, (p) => SelectResult(p));
            SpeechTextCommand = new RelayCommand<string>((p) => !string.IsNullOrWhiteSpace(p), (p) => _synthesizer.SpeakAsync(p));
            SearchWithAICommand = new RelayCommand<string>((p) => true, async (p) =>
            {
                string searchText = p;

                if (string.IsNullOrWhiteSpace(p))
                    searchText = SelectedWordResult.Word;

                IsLoadingAI = true;
                AIResultMessage = "Loading...";
                AIResult = null;

                try
                {
                    var wordResult = await _aiService.SearchWordWithAIAsync(searchText);

                    if (wordResult != null)
                    {
                        AIResult = wordResult;
                        AIResultMessage = null;
                    }
                    else
                    {
                        AIResult = null;
                        AIResultMessage = "🤖 Không tìm thấy kết quả từ AI.";
                    }
                }
                catch (Exception ex)
                {
                    AIResult = null;
                    AIResultMessage = $"❌ Lỗi khi tìm kiếm từ với AI: {ex.Message}";
                }
                finally
                {
                    IsLoadingAI = false;
                }
            });

            LoadData();
            IndexData();
            SetupSpeechSynthesizer();
        }

        /// <summary>
        /// Updates the suggestions based on the search text.
        /// </summary>
        private void UpdateSuggestions()
        {
            DictionarySearchResultDTOs.Clear();

            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var matches = _allWords.AsParallel() // Use AsParallel for parallel processing
                                   .Where(w => w.Content.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase)) // case insensitive
                                   .Take(10) // limit to 10 results
                                   .ToList();

            foreach (var match in matches)
            {
                string posStr = "";
                string defStr = "";

                if (_posByWordId.TryGetValue(match.Id, out var posList))
                {
                    var pos = posList.FirstOrDefault();
                    posStr = pos?.Content ?? ""; // Default value when pos is null

                    if (pos != null && _definitionsByPosId.TryGetValue(pos.Id, out var defs))
                    {
                        defStr = defs.FirstOrDefault()?.Content ?? ""; // Default value when definition is null
                    }
                }

                DictionarySearchResultDTOs.Add(new DictionarySearchResultDTO
                {
                    WordId = match.Id,
                    Word = match.Content,
                    Pos = posStr, // Default value when pos is null
                    Meaning = defStr // Default value when definition is null
                });
            }
        }

        /// <summary>
        /// Handles the selection of a search result.
        /// </summary>
        /// <param name="selected"></param>
        private void SelectResult(DictionarySearchResultDTO selected)
        {
            // Handle the selection of a search result
            var selectedWord = _allWords.FirstOrDefault(w => w.Id == selected.WordId); 

            if (selectedWord == null)
            {
                MessageBox.Show("Word not found.");
                return;
            }

            
            // Create the WordResultDTO object with the selected word and its pos
            SelectedWordResult = BuildWordResultDTO(selectedWord);
            SearchText = "";
            DictionarySearchResultDTOs.Clear();
        }

        /// <summary>
        /// Builds the WordResultDTO object for the selected word.
        /// </summary>
        /// <param name="word">Selected word</param>
        /// <returns></returns>
        private WordResultDTO BuildWordResultDTO(Word word)
        {
            // Get all pos for the selected word
            var posDTOs = new List<PosDTO>();

            if (_posByWordId.TryGetValue(word.Id, out var posList))
            {
                foreach (var pos in posList)
                {
                    var defDTOs = new List<DefinitionDTO>();

                    if (_definitionsByPosId.TryGetValue(pos.Id, out var defList))
                    {
                        foreach (var def in defList)
                        {
                            var examples = new List<ExampleDTO>();
                            if (_examplesByDefId.TryGetValue(def.Id, out var exList))
                            {
                                foreach (var ex in exList)
                                {
                                    var parts = ex.Content.Split(new[] { '+' }, 2); // Split into two parts
                                    examples.Add(new ExampleDTO
                                    {
                                        Id = ex.Id,
                                        DefId = ex.DefId,
                                        Example = parts[0].Trim(),
                                        Translation = parts.Length > 1 ? parts[1].Trim() : ""
                                    });
                                }
                            }

                            defDTOs.Add(new DefinitionDTO
                            {
                                Id = def.Id,
                                PosId = def.PosId,
                                Definition = def.Content,
                                Examples = examples
                            });
                        }
                    }

                    posDTOs.Add(new PosDTO
                    {
                        Id = pos.Id,
                        WordId = pos.WordId,
                        Pos = pos.Content,
                        Definitions = defDTOs
                    });
                }
            }

            return new WordResultDTO
            {
                Id = word.Id,
                Word = word.Content,
                Pronunciation = word.Prononciation,
                Pos = posDTOs
            };
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

        /// <summary>
        /// Indexes the data for faster access.
        /// </summary>
        private void IndexData()
        {
            _posByWordId = _allPos.GroupBy(p => p.WordId)
                                  .ToDictionary(g => g.Key, g => g.ToList());

            _definitionsByPosId = _allDefinitions.GroupBy(d => d.PosId)
                                         .ToDictionary(g => g.Key, g => g.ToList());

            _examplesByDefId = _allExamples.GroupBy(e => e.DefId)
                                          .ToDictionary(g => g.Key, g => g.ToList());
        }

        private void SetupSpeechSynthesizer()
        {
            // Initialize the SpeechSynthesizer
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SelectVoice("Microsoft Zira Desktop"); // Select a voice
            _synthesizer.Volume = 100; // Set volume (0-100)
            _synthesizer.Rate = 0; // Set rate (-10 to 10)
        }
    }
}
