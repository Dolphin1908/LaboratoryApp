using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Modules.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Core.Caches;

namespace LaboratoryApp.src.Modules.Chemistry.CompoundFunction.ViewModels
{
    public class CompoundManagerViewModel : BaseViewModel
    {
        private string _searchText;
        private Compound _selectedCompound;

        private ObservableCollection<Compound> _compounds;
        private List<Compound> _allCompounds; // Assuming you have a list of all compounds to search from

        #region Commands
        public ICommand AddCompoundCommand { get; set; }
        public ICommand SelectResultCommand { get; set; }
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
        public Compound SelectedCompound
        {
            get => _selectedCompound;
            set
            {
                _selectedCompound = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Compound> Compounds
        {
            get => _compounds;
            set
            {
                _compounds = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public CompoundManagerViewModel()
        {
            _compounds = new ObservableCollection<Compound>();

            _allCompounds = ChemistryDataCache.AllCompounds; // Assuming you have a data cache or repository to get all compounds

            AddCompoundCommand = new RelayCommand<object>(p => true, p =>
            {
                var addCompoundWindow = new AddCompoundWindow
                {
                    DataContext = new CompoundViewModel()
                };
                addCompoundWindow.ShowDialog();
            });
            SelectResultCommand = new RelayCommand<object>(p => true, p => 
            {
                SelectedCompound = (Compound)p;
                SearchText = string.Empty;
            });
        }

        public void UpdateSuggestions()
        {
            Compounds.Clear();

            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var matches = _allCompounds.AsParallel()
                                       .Where(c => c.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                                       .Take(10)
                                       .ToList();

            foreach (var match in matches)
            {
                Compounds.Add(match);
            }
        }
    }
}
