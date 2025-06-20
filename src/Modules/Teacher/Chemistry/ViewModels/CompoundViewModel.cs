﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Modules.Teacher.Chemistry.Views;
using LaboratoryApp.src.Core.Caches;

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.ViewModels
{
    public class CompoundViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        private List<Element> _allElements;
        private ObservableCollection<Compound> _allCompounds;
        private List<string> _allUnits;

        private Compound _compound;
        private ObservableCollection<CompoundElement> _composition;
        private ObservableCollection<PhysicalProperty> _physicalProperties;
        private ObservableCollection<ChemicalProperty> _chemicalProperties;
        private ObservableCollection<CompoundNoteViewModel> _notes;

        private ChemistryService _chemistryService;

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
        public ObservableCollection<CompoundElement> Composition
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

        public CompoundViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // Khởi tạo các collection
            Compound = new Compound();
            Composition = new ObservableCollection<CompoundElement>();
            PhysicalProperties = new ObservableCollection<PhysicalProperty>();
            ChemicalProperties = new ObservableCollection<ChemicalProperty>();
            Notes = new ObservableCollection<CompoundNoteViewModel>();

            _chemistryService = new ChemistryService();

            AllElements = ChemistryDataCache.AllElements;
            AllCompounds = new ObservableCollection<Compound>(ChemistryDataCache.AllCompounds);
            AllUnits = ["g/mol", "°C", "K", "g/cm³", "kg/m³", "g/L", "J/(g·K)", "J/(kg·K)", "mmHg", "Pa", "kPa", "MPa", "atm", "bar", "Torr", "S/m", "g/100 mL", "mol/L", "mg/L", "kJ/mol", "J/g", "Pa·s", "cP"];
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
                p => CanAddDefaultElement(),
                p =>
                {
                    Composition.Add(new CompoundElement
                    {
                        Element = AllElements.FirstOrDefault(e => !Composition.Any(c => c.Element.Id == e.Id)) ?? new Element(),
                        Quantity = "1"
                    });

                    // ép re-evaluate CanExecute của tất cả command
                    CommandManager.InvalidateRequerySuggested();
                });
            RemoveElementCommand = new RelayCommand<object>(p => true, p =>
            {
                if (p is CompoundElement element)
                {
                    Composition.Remove(element);
                }
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
                if (CanAddDefaultElement())
                {
                    Compound.Id = AllCompounds.Count + 1;
                    Compound.Composition = Composition.ToList();
                    Compound.CompoundTypes = CompoundTypeOptions.Where(x => x.IsSelected)
                                                                .Select(x => x.Value)
                                                                .ToList();
                    Compound.Phases = PhaseOptions.Where(x => x.IsSelected)
                                                  .Select(x => x.Value)
                                                  .ToList();
                    Compound.PhysicalProperties = PhysicalProperties.ToList();
                    Compound.ChemicalProperties = ChemicalProperties.ToList();
                    Compound.Notes = Notes.Select(vm => new CompoundNote
                    {
                        NoteType = vm.CompoundNoteType,
                        Content = vm.CompoundNotes.Select(n => n.Text).ToList()
                    }).ToList();

                    _chemistryService.AddCompound(Compound);
                    ChemistryDataCache.AllCompounds.Add(Compound);

                    var thisWindow = p as AddCompoundWindow;
                    thisWindow.Close();
                }
                else
                {
                    MessageBox.Show("Có nguyên tố trùng lặp trong công thức hóa học", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
            #endregion
        }

        private bool CanSave()
        {
            if (string.IsNullOrWhiteSpace(Compound.Name))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Compound.Formula))
            {
                return false;
            }
            if (Compound.MolecularMass.Equals(0))
            {
                return false;
            }
            return true;
        }

        private bool CanAddDefaultElement()
        {
            return !Composition
                .GroupBy(c => c.Element.Id)
                .Any(g => g.Count() > 1);
        }
    }
}
