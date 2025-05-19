using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;

namespace LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionComponentViewModel : BaseViewModel
    {
        private string _searchText;
        private bool _isSuggestionOpen;
        private ObservableCollection<object> _searchResult;
        private SubstanceKind _kind;
        private object _selectedSuggestion;
        private string _stoichiometricCoefficient;

        #region Properties
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                UpdateSuggestions(_searchText);
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
        public string StoichiometricCoefficient
        {
            get => _stoichiometricCoefficient;
            set
            {
                _stoichiometricCoefficient = value;
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

        private readonly List<Element> _allElements;
        private readonly ObservableCollection<Compound> _allCompounds;

        public ReactionComponentViewModel(List<Element> allElements, ObservableCollection<Compound> allCompounds)
        {
            _allElements = allElements;
            _allCompounds = allCompounds;
            SearchResult = new ObservableCollection<object>();
            Kind = SubstanceKind.Element;
        }

        private void UpdateSuggestions(string text)
        {
            SearchResult.Clear();

            if (string.IsNullOrEmpty(text))
            {
                IsSuggestionOpen = false;
                return;
            }

            if (Kind == SubstanceKind.Element)
            {
                var elements = _allElements
                    .Where(e => e.Name.Contains(text, StringComparison.OrdinalIgnoreCase) || e.Formula.Contains(text, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (var element in elements)
                {
                    SearchResult.Add(element);
                }
            }
            else if (Kind == SubstanceKind.Compound)
            {
                var compounds = _allCompounds
                    .Where(c => c.Formula.Contains(text, StringComparison.OrdinalIgnoreCase))
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
    }
}
