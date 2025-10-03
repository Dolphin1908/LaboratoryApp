using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Services.Counter;
using LaboratoryApp.src.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels
{
    public class CompoundViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IChemistryService _chemistryService;
        private readonly ICounterService _counterService;

        private List<string> _allUnits;
        private ObservableCollection<Element> _allElements;
        private ObservableCollection<Compound> _allCompounds;

        private Compound _compound;
        private ObservableCollection<CompoundComponentViewModel> _composition;
        private ObservableCollection<PhysicalProperty> _physicalProperties;
        private ObservableCollection<ChemicalProperty> _chemicalProperties;
        private ObservableCollection<CompoundNoteViewModel> _notes;

        #region Commands
        public ICommand AddElementCommand { get; set; }
        public ICommand RemoveElementCommand { get; set; }
        public ICommand AddPhysicalPropertyCommand { get; set; }
        public ICommand RemovePhysicalPropertyCommand { get; set; }
        public ICommand AddChemicalPropertyCommand { get; set; }
        public ICommand RemoveChemicalPropertyCommand { get; set; }
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
        public List<string> AllUnits
        {
            get => _allUnits;
            set
            {
                _allUnits = value;
                OnPropertyChanged();
            }
        }
        public string SelectedCompoundTypeNames
        {
            get
            {
                var names = CompoundTypeOptions
                    .Where(x => x.IsSelected)
                    .Select(x => x.DisplayName);
                return names.Any() ? string.Join("\n", names) : string.Empty;
            }
        }
        public string SelectedPhaseNames
        {
            get
            {
                var names = PhaseOptions
                    .Where(x => x.IsSelected)
                    .Select(x => x.DisplayName);
                return names.Any() ? string.Join("\n", names) : string.Empty;
            }
        }
        public ObservableCollection<SelectableEnumDisplay<CompoundType>> CompoundTypeOptions { get; }
        public ObservableCollection<SelectableEnumDisplay<ChemicalPhase>> PhaseOptions { get; }
        public Compound Compound
        {
            get => _compound;
            set
            {
                _compound = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CompoundComponentViewModel> Composition
        {
            get => _composition;
            set
            {
                _composition = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PhysicalProperty> PhysicalProperties
        {
            get => _physicalProperties;
            set
            {
                _physicalProperties = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ChemicalProperty> ChemicalProperties
        {
            get => _chemicalProperties;
            set
            {
                _chemicalProperties = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CompoundNoteViewModel> Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public CompoundViewModel(IServiceProvider serviceProvider,
                                 IChemistryService chemistryService,
                                 ICounterService counterService)
        {
            _serviceProvider = serviceProvider;
            _chemistryService = chemistryService;
            _counterService = counterService;

            // Khởi tạo các thuộc tính
            _allUnits = new List<string>();
            _allElements = new ObservableCollection<Element>();
            _allCompounds = new ObservableCollection<Compound>();

            // Khởi tạo các collection
            _compound = new Compound();
            _composition = new ObservableCollection<CompoundComponentViewModel>();
            _physicalProperties = new ObservableCollection<PhysicalProperty>();
            _chemicalProperties = new ObservableCollection<ChemicalProperty>();
            _notes = new ObservableCollection<CompoundNoteViewModel>();

            _allUnits = ["g/mol", "°C", "K", "g/cm³", "kg/m³", "g/L", "J/(g·K)", "J/(kg·K)", "mmHg", "Pa", "kPa", "MPa", "atm", "bar", "Torr", "S/m", "g/100 mL", "mol/L", "mg/L", "kJ/mol", "J/g", "Pa·s", "cP"];
            CompoundTypeOptions = new ObservableCollection<SelectableEnumDisplay<CompoundType>>(
                Enum.GetValues(typeof(CompoundType))
                    .Cast<CompoundType>()
                    .Select(e => new SelectableEnumDisplay<CompoundType>(e))
            );
            PhaseOptions = new ObservableCollection<SelectableEnumDisplay<ChemicalPhase>>(
                Enum.GetValues(typeof(ChemicalPhase))
                    .Cast<ChemicalPhase>()
                    .Select(e => new SelectableEnumDisplay<ChemicalPhase>(e))
            );

            #region Events
            // Đăng ký sự kiện cho các thuộc tính CompoundTypeOptions
            foreach (var item in CompoundTypeOptions)
            {
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(item.IsSelected))
                        OnPropertyChanged(nameof(SelectedCompoundTypeNames));
                };
            }

            // Đăng ký sự kiện cho các thuộc tính PhaseOptions
            foreach (var item in PhaseOptions)
            {
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(item.IsSelected))
                        OnPropertyChanged(nameof(SelectedPhaseNames));
                };
            }
            #endregion

            #region Commands
            AddElementCommand = new RelayCommand<object>(
                p => CanAddDefaultElement(Composition),
                p =>
                {
                    var vm = _serviceProvider.GetRequiredService<CompoundComponentViewModel>();
                    if (vm is CompoundComponentViewModel compoundComponentVM && compoundComponentVM is IAsyncInitializable init)
                        _ = init.InitializeAsync();
                    Composition.Add(vm);
                });
            RemoveElementCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is CompoundComponentViewModel element)
                    Composition.Remove(element);
            });

            AddPhysicalPropertyCommand = new RelayCommand<object>(p => true, p =>
            {
                PhysicalProperties.Add(new PhysicalProperty());
            });
            RemovePhysicalPropertyCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is PhysicalProperty property)
                {
                    PhysicalProperties.Remove(property);
                }
            });

            AddChemicalPropertyCommand = new RelayCommand<object>(p => true, p =>
            {
                ChemicalProperties.Add(new ChemicalProperty());
            });
            RemoveChemicalPropertyCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is ChemicalProperty property)
                {
                    ChemicalProperties.Remove(property);
                }
            });

            AddNoteGroupCommand = new RelayCommand<object>(p => true, p =>
            {
                Notes.Add(_serviceProvider.GetRequiredService<CompoundNoteViewModel>());
            });
            RemoveNoteGroupCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is CompoundNoteViewModel note)
                {
                    Notes.Remove(note);
                }
            });

            SaveCommand = new RelayCommand<object>(p => CanSave(), p =>
            {
                if (CanAddDefaultElement(Composition))
                {
                    SaveCompound();

                    if (p is AddCompoundWindow thisWindow)
                    {
                        thisWindow.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Có nguyên tố trùng lặp trong công thức hóa học", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
            #endregion
        }

        private void SaveCompound()
        {
            Compound.Composition = Composition
                .Select(vm => new CompoundComponent
                {
                    ElementId = vm.SelectedElement.Id,
                    Formula = vm.SelectedElement.Formula,
                    Quantity = vm.Quantity
                })
                .ToList();

            Compound.CompoundTypes = CompoundTypeOptions.Where(x => x.IsSelected)
                                                        .Select(x => x.Value)
                                                        .ToList();
            Compound.Phases = PhaseOptions.Where(x => x.IsSelected)
                                          .Select(x => x.Value)
                                          .ToList();

            Compound.PhysicalProperties = PhysicalProperties.Select(p => new PhysicalProperty
                                                            { 
                                                                PropertyName = p.PropertyName,
                                                                Unit = string.IsNullOrWhiteSpace(p.Unit) ? null : p.Unit,
                                                                Value = string.IsNullOrWhiteSpace(p.Value) ? null : p.Value,
                                                                Description = string.IsNullOrWhiteSpace(p.Description) ? null : p.Description
                                                            })
                                                            .ToList();

            Compound.ChemicalProperties = ChemicalProperties.ToList();

            Compound.Notes = Notes.Select(vm => new CompoundNote
            {
                NoteType = vm.CompoundNoteType,
                Content = vm.CompoundNotes.Select(n => n.Text)
                                          .Where(cn => !string.IsNullOrWhiteSpace(cn))
                                          .ToList()
            }).ToList();

            Compound.Id = _counterService.GetNextId(CollectionName.Compounds);

            _chemistryService.AddCompound(Compound);
            ChemistryDataCache.AllCompounds.Add(Compound);
        }

        private bool CanAddDefaultElement(ObservableCollection<CompoundComponentViewModel> list)
        {
            var elements = list.Select(vm => vm.SelectedElement)
                               .Where(item => item != null)
                               .Where(e => !string.IsNullOrEmpty(e.Formula))
                               .ToList();

            return !elements.GroupBy(e => e)
                            .Any(g => g.Count() > 1);
        }

        private bool CanSave()
        {
            return Compound != null && // Kiểm tra Compound không null
                   !string.IsNullOrWhiteSpace(Compound.Name) && !string.IsNullOrWhiteSpace(Compound.Formula) && !Compound.MolecularMass.Equals(0) && // Kiểm tra các thuộc tính bắt buộc
                   Composition.Count > 0 && Composition.Any(c => c.SelectedElement != null && !string.IsNullOrWhiteSpace(c.Quantity)) &&
                   (PhysicalProperties.Count == 0 || PhysicalProperties.All(pp => !string.IsNullOrWhiteSpace(pp.PropertyName) && (!string.IsNullOrWhiteSpace(pp.Value) || !string.IsNullOrWhiteSpace(pp.Description)))) &&
                   (ChemicalProperties.Count == 0 || ChemicalProperties.All(cp => !string.IsNullOrWhiteSpace(cp.PropertyName) && !string.IsNullOrWhiteSpace(cp.Description))) &&
                   (Notes.Count == 0 || Notes.Any(n => n.CompoundNotes.Any(cn => !string.IsNullOrWhiteSpace(cn.Text)))) &&
                   CompoundTypeOptions.Any(c => c.IsSelected) &&
                   PhaseOptions.Any(p => p.IsSelected) &&
                   CanAddDefaultElement(Composition);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            // Load initial data if needed
            await Task.Run(() =>
            {
                _allElements = new ObservableCollection<Element>(ChemistryDataCache.AllElements);
                _allCompounds = new ObservableCollection<Compound>(ChemistryDataCache.AllCompounds);
            }, cancellationToken);
        }
    }
}
