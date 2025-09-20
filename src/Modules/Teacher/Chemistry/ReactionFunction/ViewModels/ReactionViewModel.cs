using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels;

using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Core.Models.Chemistry.Common.Enums;

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IChemistryService _chemistryService;

        private ObservableCollection<Element> _allElements;
        private ObservableCollection<Compound> _allCompounds;
        private ObservableCollection<Reaction> _allReactions;

        private ObservableCollection<ReactionOtherCondition> _otherConditions;

        private Reaction _reaction;
        private ObservableCollection<ReactionComponentViewModel> _reactants;
        private ObservableCollection<ReactionComponentViewModel> _products;
        private ObservableCollection<ReactionNoteViewModel> _notes;


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
        public ObservableCollection<Element> AllElements
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

        public ReactionViewModel(IChemistryService chemistryService,
                                 IServiceProvider serviceProvider)
        {
            _chemistryService = chemistryService;
            _serviceProvider = serviceProvider;

            _allElements = new ObservableCollection<Element>();
            _allCompounds = new ObservableCollection<Compound>();
            _allReactions = new ObservableCollection<Reaction>();

            _reaction = new Reaction();
            _otherConditions = new ObservableCollection<ReactionOtherCondition>();
            _reactants = new ObservableCollection<ReactionComponentViewModel>();
            _products = new ObservableCollection<ReactionComponentViewModel>();
            _notes = new ObservableCollection<ReactionNoteViewModel>();

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
                    var vm = _serviceProvider.GetRequiredService<ReactionComponentViewModel>();
                    if (vm is ReactionComponentViewModel reactantComponentVM && reactantComponentVM is IAsyncInitializable init)
                        _ = init.InitializeAsync();
                    Reactants.Add(vm);
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
                    var vm = _serviceProvider.GetRequiredService<ReactionComponentViewModel>();
                    if (vm is ReactionComponentViewModel productComponentVM && productComponentVM is IAsyncInitializable init)
                        _ = init.InitializeAsync();
                    Products.Add(vm);
                });
            RemoveProductCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is ReactionComponentViewModel product)
                    Products.Remove(product);
            });

            AddNoteGroupCommand = new RelayCommand<object>(p => true, p =>
            {
                Notes.Add(_serviceProvider.GetRequiredService<ReactionNoteViewModel>());
            });
            RemoveNoteGroupCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is ReactionNoteViewModel note)
                    Notes.Remove(note);
            });

            SaveCommand = new RelayCommand<object>(p => CanSave(), p =>
            {
                Reaction.Id = AllReactions.Max(r => r.Id) + 1;

                Reaction.ReactionType = ReactionTypeOptions.Where(e => e.IsSelected)
                                                           .Select(e => e.Value)
                                                           .ToList();

                Reaction.Reactants = Reactants
                .Select(vm => new ReactionComponent
                {
                    Kind = vm.Kind,
                    ElementId = vm.SelectedElement.Id,
                    CompoundId = vm.SelectedCompound.Id,
                    Coefficient = vm.Coefficient
                })
                .ToList();

                Reaction.Products = Products
                .Select(vm => new ReactionComponent
                {
                    Kind = vm.Kind,
                    ElementId = vm.SelectedElement.Id,
                    CompoundId = vm.SelectedCompound.Id,
                    Coefficient = vm.Coefficient
                })
                .ToList();

                Reaction.ReactionNotes = Notes.Select(vm => new ReactionNote
                {
                    NoteType = vm.ReactionNoteType,
                    Content = vm.ReactionNotes.Select(n => n.Text).ToList()
                }).ToList();

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

        /// <summary>
        /// Kiểm tra xem có đủ thông tin để lưu không
        /// </summary>
        /// <returns></returns>
        private bool CanSave()
        {
            // Kiểm tra xem có đủ thông tin để lưu không
            return Reaction != null &&
                   Reactants.Count > 0 &&
                   Products.Count > 0 &&
                   ReactionTypeOptions.Any(e => e.IsSelected);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _allElements = new ObservableCollection<Element>(ChemistryDataCache.AllElements);
                _allCompounds = new ObservableCollection<Compound>(ChemistryDataCache.AllCompounds);
                _allReactions = new ObservableCollection<Reaction>(ChemistryDataCache.AllReactions);
            }, cancellationToken);
        }
    }
}
