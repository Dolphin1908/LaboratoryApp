using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction.DTOs;

using LaboratoryApp.src.Services.English.DictionaryFunction;

using LaboratoryApp.src.Services.Helper.AI;
using LaboratoryApp.src.Services.Helper.Speech;

namespace LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels
{
    public class DictionaryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IAIService _aiService;
        private readonly IDictionaryService _dictionaryService;

        // Private fields for search and AI results
        private string? _searchText;
        private string? _aiResultMessage;
        private bool _isLoadingAI;
        private WordResultDTO? _aiResult;

        // Selected word result
        private WordResultDTO? _selectedWordResult;
        private ObservableCollection<DictionarySearchResultDTO> _dictionarySearchResultDTOs;

        #region Commands
        public ICommand SelectResultCommand { get; set; }
        public ICommand SpeechTextCommand { get; set; }
        public ICommand SearchWithAICommand { get; set; }
        #endregion

        #region Properties
        public string SearchText
        {
            get => _searchText ?? string.Empty;
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
        public WordResultDTO? SelectedWordResult
        {
            get => _selectedWordResult;
            set
            {
                _selectedWordResult = value;
                OnPropertyChanged();
            }
        }
        public WordResultDTO? AIResult
        {
            get => _aiResult;
            set
            {
                _aiResult = value;
                OnPropertyChanged();
            }
        }
        public string? AIResultMessage
        {
            get => _aiResultMessage ?? string.Empty;
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
                                   IDictionaryService dictionaryService)
        {
            _aiService = aiService;
            _dictionaryService = dictionaryService;

            // Initialize commands and properties here if needed
            _dictionarySearchResultDTOs = new ObservableCollection<DictionarySearchResultDTO>();

            SelectResultCommand = new RelayCommand<DictionarySearchResultDTO>((p) => p != null, (p) => SelectResult(p));
            SpeechTextCommand = new RelayCommand<string>((p) => !string.IsNullOrWhiteSpace(p) && SpeechService.synthesizer != null, (p) =>
            {
                if (SpeechService.synthesizer != null)
                    SpeechService.synthesizer.SpeakAsync(p);
            });
            SearchWithAICommand = new RelayCommand<string>((p) => true, async (p) =>
            {
                string searchText = p;

                if (string.IsNullOrWhiteSpace(p))
                    searchText = SelectedWordResult?.Word ?? string.Empty;

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
            DictionarySearchResultDTOs.Clear();

            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var suggestions = _dictionaryService.GetSuggestions(SearchText, 10);
            foreach (var suggestion in suggestions)
            {
                DictionarySearchResultDTOs.Add(suggestion);
            }
        }

        /// <summary>
        /// Handles the selection of a search result.
        /// </summary>
        /// <param name="selected"></param>
        private void SelectResult(DictionarySearchResultDTO selected)
        {
            // Handle the selection of a search result
            var selectedWord = _dictionaryService.GetWordById(selected.WordId);

            if (selectedWord == null)
            {
                MessageBox.Show("Word not found.");
                return;
            }

            // Create the WordResultDTO object with the selected word and its pos
            SelectedWordResult = _dictionaryService.BuildWordResultDTO(selectedWord);
            SearchText = "";
            DictionarySearchResultDTOs.Clear();
        }
    }
}
