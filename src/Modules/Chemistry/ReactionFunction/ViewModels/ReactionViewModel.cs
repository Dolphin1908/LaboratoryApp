using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Services.Chemistry;

namespace LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionViewModel : BaseViewModel
    {
        private List<Element> _allElements;
        private ObservableCollection<Compound> _allCompounds;
        private ObservableCollection<Reaction> _allReactions;

        private ObservableCollection<ReactionOtherCondition> _otherConditions;

        private Reaction _reaction;
        private ObservableCollection<ReactionComponentViewModel> _reactants;
        private ObservableCollection<ReactionComponentViewModel> _products;
        private ObservableCollection<ReactionNoteViewModel> _notes;

        private ChemistryService _chemistryService;

        #region Commands
        public ICommand AddOtherConditionCommand { get; set; }
        public ICommand RemoveOtherConditionCommand { get; set; }
        public ICommand AddReactantCommand { get; set; }
        public ICommand RemoveReactantCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand AddNoteGroupCommand { get; set; }
        public ICommand RemoveNoteGroupCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

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
        public ObservableCollection<Reaction> AllReactions
        {
            get => _allReactions;
            set
            {
                _allReactions = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ReactionOtherCondition> OtherConditions
        {
            get => _otherConditions;
            set
            {
                _otherConditions = value;
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
                return names.Any() ? string.Join("", names) : string.Empty;
            }
        }
        public ReactionComponent SelectedReactant { get; set; }
        public SubstanceKind SelectedReactantKind { get; set; }

        public ObservableCollection<SelectableEnumDisplay<ReactionType>> ReactionTypeOptions { get; }
        public ObservableCollection<EnumDisplay<ReactionNoteType>> ReactionNoteTypeOptions { get; }
        public ObservableCollection<EnumDisplay<SubstanceKind>> SubstanceKindOptions { get; set; }

        public Reaction Reaction
        {
            get => _reaction;
            set
            {
                _reaction = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ReactionComponentViewModel> Reactants
        {
            get => _reactants;
            set
            {
                _reactants = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ReactionComponentViewModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ReactionNoteViewModel> Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ReactionViewModel()
        {
            AllElements = ChemistryDataCache.AllElements;
            AllCompounds = new ObservableCollection<Compound>(ChemistryDataCache.AllCompounds);
            AllReactions = new ObservableCollection<Reaction>(ChemistryDataCache.AllReactions);

            _chemistryService = new ChemistryService();
            Reaction = new Reaction();
            OtherConditions = new ObservableCollection<ReactionOtherCondition>();
            Reactants = new ObservableCollection<ReactionComponentViewModel>();
            Products = new ObservableCollection<ReactionComponentViewModel>();
            Notes = new ObservableCollection<ReactionNoteViewModel>();

            ReactionTypeOptions = new ObservableCollection<SelectableEnumDisplay<ReactionType>>(
                Enum.GetValues(typeof(ReactionType))
                    .Cast<ReactionType>()
                    .Select(e => new SelectableEnumDisplay<ReactionType>(e))
            );
            SubstanceKindOptions = new ObservableCollection<EnumDisplay<SubstanceKind>>(
                Enum.GetValues(typeof(SubstanceKind))
                    .Cast<SubstanceKind>()
                    .Select(e => new EnumDisplay<SubstanceKind>(e))
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

            #region Commands
            AddOtherConditionCommand = new RelayCommand<object>(p => true, p =>
            {
                OtherConditions.Add(new ReactionOtherCondition());
            });
            RemoveOtherConditionCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is ReactionOtherCondition condition)
                {
                    OtherConditions.Remove(condition);
                }
            });

            AddReactantCommand = new RelayCommand<object>(
                p => CanAddRP(Reactants),
                p =>
                {
                    Reactants.Add(new ReactionComponentViewModel(AllElements, AllCompounds));
                });
            RemoveReactantCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is ReactionComponentViewModel reactant)
                    Reactants.Remove(reactant);
            });

            AddProductCommand = new RelayCommand<object>(
                p => CanAddRP(Products),
                p =>
                {
                    Products.Add(new ReactionComponentViewModel(AllElements, AllCompounds));
                });
            RemoveProductCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is ReactionComponentViewModel product)
                    Products.Remove(product);
            });

            AddNoteGroupCommand = new RelayCommand<object>(p => true, p =>
            {
                Notes.Add(new ReactionNoteViewModel());
            });
            RemoveNoteGroupCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is ReactionNoteViewModel note)
                    Notes.Remove(note);
            });

            SaveCommand = new RelayCommand<object>(p => true, p =>
            {
                Reaction.Id = AllReactions.Count + 1;

                Reaction.ReactionType = ReactionTypeOptions.Where(e => e.IsSelected)
                                                           .Select(e => e.Value)
                                                           .ToList();

                Reaction.Reactants = Reactants
                .Select(vm => new ReactionComponent
                {
                    Kind = vm.Kind,
                    Element = vm.SelectedElement,
                    Compound = vm.SelectedCompound,
                    StoichiometricCoefficient = vm.StoichiometricCoefficient
                })
                .ToList();

                Reaction.Products = Products
                .Select(vm => new ReactionComponent
                {
                    Kind = vm.Kind,
                    Element = vm.SelectedElement,
                    Compound = vm.SelectedCompound,
                    StoichiometricCoefficient = vm.StoichiometricCoefficient
                })
                .ToList();

                Reaction.ReactionNotes = Notes.Select(vm => new ReactionNote
                {
                    NoteType = vm.ReactionNoteType,
                    Content = vm.ReactionNotes.Select(n => n.Text).ToList()
                }).ToList();

                if (Reaction.Condition.Catalyst == string.Empty)
                    Reaction.Condition.Catalyst = "Không có";
                if (Reaction.Condition.Temperature == string.Empty)
                    Reaction.Condition.Temperature = "Không có";
                if (Reaction.Condition.Pressure == string.Empty)
                    Reaction.Condition.Pressure = "Không có";
                if (Reaction.Condition.Solvent == string.Empty)
                    Reaction.Condition.Solvent = "Không có";
                if (Reaction.Condition.PH == string.Empty)
                    Reaction.Condition.PH = "Không có";

                Reaction.Condition.OtherConditions = OtherConditions.ToList();

                _chemistryService.AddReaction(Reaction);
                ChemistryDataCache.AllReactions.Add(Reaction);

                var thisWindow = p as AddReactionWindow;
                thisWindow.Close();
            });
            #endregion
        }

        /// <summary>
        /// Kiểm tra có đơn chất hay hợp chất nào trùng lặp trong danh sách không
        /// </summary>
        /// <returns></returns>
        private bool CanAddRP(ObservableCollection<ReactionComponentViewModel> list)
        {
            var formulas = list.Select(vm => vm.SelectedSuggestion)
                               .Where(item => item != null)
                               .Select(item =>
                               {
                                   return item switch
                                   {
                                       Element e => e.Formula,
                                       Compound c => c.Formula,
                                       _ => null
                                   };
                               })
                               .Where (f=> !string.IsNullOrEmpty(f))
                               .ToList();

            return !formulas.GroupBy(f => f)
                            .Any(g => g.Count() > 1);
        }
    }
}
