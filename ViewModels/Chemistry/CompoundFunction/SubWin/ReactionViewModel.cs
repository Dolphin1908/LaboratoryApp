using LaboratoryApp.Models.Chemistry;
using LaboratoryApp.Models.Chemistry.Enums;
using LaboratoryApp.Support.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.ViewModels.Chemistry.CompoundFunction.SubWin
{
    public class ReactionViewModel : BaseViewModel
    {
        private List<Element> _allElements;
        private ObservableCollection<Compound> _allCompounds;

        #region Properties
        public List<Element> AllElements
        {
            get => _allElements;
            set
            {
                _allElements = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Compound> AllCompounds
        {
            get => _allCompounds;
            set
            {
                _allCompounds = value;
                OnPropertyChanged();
            }
        }
        public string SelectedReactionTypeNames
        {
            get
            {
                var names = ReactionTypeOptions
                    .Where(e => e.IsSelected)
                    .Select(e => e.DisplayName);
                return names.Any() ? string.Join("\n", names) : String.Empty;
            }
        }
        public ObservableCollection<SelectableEnumDisplay<ReactionType>> ReactionTypeOptions { get; }
        #endregion

        public ReactionViewModel()
        {
            AllElements = ChemistryDataCache.AllElements;
            AllCompounds = new ObservableCollection<Compound>(ChemistryDataCache.AllCompounds);
            ReactionTypeOptions = new ObservableCollection<SelectableEnumDisplay<ReactionType>>(
                Enum.GetValues(typeof(ReactionType))
                    .Cast<ReactionType>()
                    .Select(e => new SelectableEnumDisplay<ReactionType>(e))
            );

            // Đăng ký sự kiện cho các thuộc tính ReactionTypeOptions
            foreach (var item in ReactionTypeOptions)
            {
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(SelectableEnumDisplay<ReactionType>.IsSelected))
                        OnPropertyChanged(nameof(SelectedReactionTypeNames));
                };
            }
        }
    }
}
