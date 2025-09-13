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
        private readonly ChemistryDataCache _chemistryDataCache;
        private readonly IChemistryService _chemistryService;
        private readonly IServiceProvider _serviceProvider;

        private string _searchText;
        private bool _isSuggestionOpen;
        private object _selectedSuggestion;
        private decimal _coefficient;
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
        public decimal Coefficient
        {
            get => _coefficient;
            set
            {
                _coefficient = value;
                OnPropertyChanged();
            }
        }
        #endregion

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
        public Element SelectedElement { get; set; }
        public Compound SelectedCompound { get; set; }

        private ObservableCollection<Element> _allElements;
        private ObservableCollection<Compound> _allCompounds;

        public ReactionComponentViewModel(ChemistryDataCache chemistryDataCache,
                                          IChemistryService chemistryService,
                                          IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _chemistryDataCache = chemistryDataCache;
            _chemistryService = chemistryService;

            _allElements = new ObservableCollection<Element>();
            _allCompounds = new ObservableCollection<Compound>();

            SearchResult = new ObservableCollection<object>();
            Kind = SubstanceKind.Element;
        }

        private void UpdateSuggestions()
        {
            SearchResult?.Clear();

            if (string.IsNullOrEmpty(SearchText))
            {
                IsSuggestionOpen = false;
                return;
            }

            if (Kind == SubstanceKind.Element)
            {
                var elements = _allElements
                    .Where(e => e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || e.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (var element in elements)
                {
                    SearchResult.Add(element);
                }
            }
            else if (Kind == SubstanceKind.Compound)
            {
                var compounds = _allCompounds
                    .Where(c => c.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (var compound in compounds)
                {
                    SearchResult.Add(compound);
                }
            }

            IsSuggestionOpen = SearchResult.Any();
        }

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
                _chemistryDataCache.LoadAllData(_chemistryService);

                _allElements = new ObservableCollection<Element>(_chemistryDataCache.AllElements);
                _allCompounds = new ObservableCollection<Compound>(_chemistryDataCache.AllCompounds);
            }, cancellationToken);

            // If there is a search text, update suggestions immediately
            UpdateSuggestions();
        }
    }
}
