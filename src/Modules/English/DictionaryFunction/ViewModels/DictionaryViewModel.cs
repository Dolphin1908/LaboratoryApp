using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Synthesis;
using System.Windows.Input;
using System.Windows;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction.DTOs;
using LaboratoryApp.src.Services.AI;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Services.English;

namespace LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels
{
    class DictionaryViewModel : BaseViewModel, IAsyncInitializable, INotifyPropertyChanged
    {
        // Dependencies
        private readonly IEnglishService _englishService;

        // Private fields for search and AI results
        private string _searchText;
        private string _aiResultMessage;
        private bool _isLoadingAI;
        private WordResultDTO _aiResult;

        // Selected word result
        private WordResultDTO _selectedWordResult;
        private ObservableCollection<DictionarySearchResultDTO> _dictionarySearchResultDTOs;

        // Collections for all words, pos, definitions, and examples
        private List<Word> _allWords;
        private List<Pos> _allPos;
        private List<Example> _allExamples;
        private List<Definition> _allDefinitions;

        // Dictionaries for fast access
        private Dictionary<long, List<Pos>> _posByWordId;
        private Dictionary<long, List<Definition>> _definitionsByPosId;
        private Dictionary<long, List<Example>> _examplesByDefId;

        // Speech synthesizer for text-to-speech functionality
        private SpeechSynthesizer _synthesizer;
        // AI service for searching words with AI
        private IAIService _aiService;

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

        public DictionaryViewModel(IAIService aiService,
                                   IEnglishService englishService)
        {
            _aiService = aiService;
            _englishService = englishService;

            // Initialize collections and dictionaries
            _allWords = new List<Word>();
            _allPos = new List<Pos>();
            _allDefinitions = new List<Definition>();
            _allExamples = new List<Example>();

            _posByWordId = new Dictionary<long, List<Pos>>();
            _definitionsByPosId = new Dictionary<long, List<Definition>>();
            _examplesByDefId = new Dictionary<long, List<Example>>();

            // Initialize commands and properties here if needed
            _dictionarySearchResultDTOs = new ObservableCollection<DictionarySearchResultDTO>();

            SelectResultCommand = new RelayCommand<DictionarySearchResultDTO>((p) => p != null, (p) => SelectResult(p));
            SpeechTextCommand = new RelayCommand<string>((p) => !string.IsNullOrWhiteSpace(p) && _synthesizer != null, (p) =>
            {
                if (_synthesizer != null)
                    _synthesizer.SpeakAsync(p);
            });
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
        }

        /// <summary>
        /// Updates the suggestions based on the search text.
        /// </summary>
        private void UpdateSuggestions()
        {
            DictionarySearchResultDTOs?.Clear();

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

                DictionarySearchResultDTOs?.Add(new DictionarySearchResultDTO
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
                Pronunciation = word.Pronunciation,
                Pos = posDTOs
            };
        }

        /// <summary>
        /// Indexes the data for faster access.
        /// </summary>
        private void IndexData()
        {
            _posByWordId = _allPos.GroupBy(p => p.WordId)
                                  .ToDictionary(g => g.Key, 
                                                g => g.ToList());

            _definitionsByPosId = _allDefinitions.GroupBy(d => d.PosId)
                                                 .ToDictionary(g => g.Key, 
                                                               g => g.ToList());

            _examplesByDefId = _allExamples.GroupBy(e => e.DefId)
                                           .ToDictionary(g => g.Key, 
                                                         g => g.ToList());
        }

        /// <summary>
        /// Sets up the SpeechSynthesizer for text-to-speech functionality.
        /// </summary>
        private void SetupSpeechSynthesizer()
        {
            try
            {
                // Initialize the SpeechSynthesizer
                _synthesizer = new SpeechSynthesizer();
                _synthesizer.SelectVoice("Microsoft Zira Desktop"); // Select a voice
                _synthesizer.Volume = 100; // Set volume (0-100)
                _synthesizer.Rate = 0; // Set rate (-10 to 10)
            }
            catch
            {
                _synthesizer = null; // If initialization fails, set to null
            }
        }

        /// <summary>
        /// Initializes the DictionaryViewModel, loading necessary data and setting up the environment.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Get all words, pos, examples, and definitions from the cache
                _allWords = EnglishDataCache.AllWords;
                _allPos = EnglishDataCache.AllPos;
                _allExamples = EnglishDataCache.AllExamples;
                _allDefinitions = EnglishDataCache.AllDefinitions;

                IndexData(); // Index the data for faster access
                SetupSpeechSynthesizer(); // Set up the SpeechSynthesizer
            }, cancellationToken);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                // If there is a search text, update suggestions immediately
                UpdateSuggestions();
            }
        }
    }
}
