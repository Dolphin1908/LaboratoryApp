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

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionComponentViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;

        private string _searchText;
        private bool _isSuggestionOpen;
        private object _selectedSuggestion;
        private string _coefficient;
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
        public object SelectedSuggestion
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

        public Element SelectedElement { get; set; }
        public Compound SelectedCompound { get; set; }

        private ObservableCollection<Element> _allElements;
        private ObservableCollection<Compound> _allCompounds;

        public ReactionComponentViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _allElements = new ObservableCollection<Element>();
            _allCompounds = new ObservableCollection<Compound>();

            SearchResult = new ObservableCollection<object>();
            Kind = SubstanceKind.Element;
        }

        /// <summary>
        /// Câp nhật danh sách gợi ý dựa trên văn bản tìm kiếm hiện tại
        /// </summary>
        private void UpdateSuggestions()
        {
            SearchResult?.Clear();

            if (string.IsNullOrEmpty(SearchText))
            {
                IsSuggestionOpen = false;
                return;
            }

            // Tìm kiếm phần tử hoặc hợp chất dựa trên loại đã chọn
            if (Kind == SubstanceKind.Element)
            {
                // Tìm kiếm theo tên hoặc công thức
                var elements = _allElements
                    .Where(e => e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || e.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                // Thêm các phần tử tìm được vào danh sách kết quả
                foreach (var element in elements)
                {
                    SearchResult.Add(element);
                }
            }
            else if (Kind == SubstanceKind.Compound)
            {
                // Tìm kiếm theo công thức
                var compounds = _allCompounds
                    .Where(c => c.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                // Thêm các hợp chất tìm được vào danh sách kết quả
                foreach (var compound in compounds)
                {
                    SearchResult.Add(compound);
                }
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

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _allElements = new ObservableCollection<Element>(ChemistryDataCache.AllElements);
                _allCompounds = new ObservableCollection<Compound>(ChemistryDataCache.AllCompounds);
            }, cancellationToken);

            // If there is a search text, update suggestions immediately
            UpdateSuggestions();
        }
    }
}
