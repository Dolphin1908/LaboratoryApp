using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Services.Chemistry.ReactionFunction;

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionComponentViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IReactionService _reactionService;

        private string _searchText = string.Empty;
        private bool _isSuggestionOpen;
        private object? _selectedSuggestion;
        private string _coefficient = string.Empty;
        private ObservableCollection<object> _searchResult;
        private SubstanceKind _kind;

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
        public bool IsSuggestionOpen
        {
            get => _isSuggestionOpen;
            set
            {
                _isSuggestionOpen = value;
                OnPropertyChanged();
            }
        }
        public object? SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                _selectedSuggestion = value;
                OnPropertyChanged();
                if (value != null)
                    SelectSuggestion(value);
            }
        }
        public string Coefficient
        {
            get => _coefficient;
            set
            {
                _coefficient = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<object> SearchResult
        {
            get => _searchResult;
            set
            {
                _searchResult = value;
                OnPropertyChanged();
            }
        }
        public SubstanceKind Kind
        {
            get => _kind;
            set
            {
                _kind = value;
                OnPropertyChanged();
                SearchText = "";
                SearchResult.Clear();
                IsSuggestionOpen = false;
            }
        }
        #endregion

        public Element? SelectedElement { get; set; }
        public Compound? SelectedCompound { get; set; }

        public ReactionComponentViewModel(IServiceProvider serviceProvider,
                                          IReactionService reactionService)
        {
            _serviceProvider = serviceProvider;
            _reactionService = reactionService;

            _searchResult = new ObservableCollection<object>();
            Kind = SubstanceKind.Element;
        }

        /// <summary>
        /// Câp nhật danh sách gợi ý dựa trên văn bản tìm kiếm hiện tại
        /// </summary>
        private void UpdateSuggestions()
        {
            SearchResult.Clear();

            if (string.IsNullOrEmpty(SearchText))
            {
                IsSuggestionOpen = false;
                return;
            }

            var matches = _reactionService.GetElementCompoundSuggestions(SearchText, Kind);
            foreach (var match in matches)
            {
                SearchResult.Add(match);
            }

            // Mở danh sách gợi ý nếu có kết quả
            IsSuggestionOpen = SearchResult.Any();
        }

        /// <summary>
        /// Xử lý khi người dùng chọn một gợi ý từ danh sách
        /// </summary>
        /// <param name="item"></param>
        public void SelectSuggestion(object item)
        {
            if (item is Element element)
                SelectedElement = element;
            else if (item is Compound compound)
                SelectedCompound = compound;

            SearchText = (item as dynamic).Formula;
            IsSuggestionOpen = false;
        }
    }
}
