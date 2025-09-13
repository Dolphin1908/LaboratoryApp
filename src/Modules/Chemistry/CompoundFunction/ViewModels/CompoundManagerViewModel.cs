using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;

using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Chemistry.CompoundFunction.ViewModels
{
    public class CompoundManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IChemistryService _chemistryService;
        private readonly ChemistryDataCache _chemistryDataCache;

        private string _searchText;
        private bool _isTeacher;
        private Compound _selectedCompound;

        private ObservableCollection<Compound> _compounds;
        private ObservableCollection<Compound> _allCompounds; // Assuming you have a list of all compounds to search from

        #region Commands
        public ICommand AddCompoundCommand { get; set; }
        public ICommand SelectCompoundCommand { get; set; }
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
        public bool IsTeacher
        {
            get => _isTeacher;
            set
            {
                _isTeacher = value;
                OnPropertyChanged(nameof(IsTeacher));
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundManagerViewModel"/> class.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="chemistryService"></param>
        /// <param name="chemistryDataCache"></param>
        public CompoundManagerViewModel(IServiceProvider serviceProvider,
                                        IChemistryService chemistryService,
                                        ChemistryDataCache chemistryDataCache)
        {
            _serviceProvider = serviceProvider;
            _chemistryService = chemistryService;
            _chemistryDataCache = chemistryDataCache;

            _compounds = new ObservableCollection<Compound>();
            _allCompounds = new ObservableCollection<Compound>();

            AuthenticationCache.CurrentUserChanged += OnUserChanged;

            AddCompoundCommand = new RelayCommand<object>(p => true, p =>
            {
                var window = _serviceProvider.GetRequiredService<AddCompoundWindow>();

                if (window.DataContext is IAsyncInitializable init)
                {
                    // Initialize the add compound window asynchronously
                    _ = init.InitializeAsync();
                }

                window.ShowDialog(); // Because this is a modal dialog, it will block the current thread until closed

                _allCompounds = new ObservableCollection<Compound>(_chemistryDataCache.AllCompounds);
            });

            SelectCompoundCommand = new RelayCommand<object>(p => true, p =>
            {
                SelectedCompound = (Compound)p;
                SearchText = string.Empty;
            });
        }

        /// <summary>
        /// Updates the suggestions based on the current search text.
        /// </summary>
        public void UpdateSuggestions()
        {
            Compounds?.Clear();

            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var matches = _allCompounds.AsParallel()
                                       .Where(c => c.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                                   c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                                       .Take(10)
                                       .ToList();

            Compounds = new ObservableCollection<Compound>(matches);
        }

        /// <summary>
        /// Initializes the view model, loading necessary data and setting up initial state.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            // Load initial data if needed
            await Task.Run(() =>
            {
                _chemistryDataCache.LoadAllData(_chemistryService);

                _allCompounds = new ObservableCollection<Compound>(_chemistryDataCache.AllCompounds);
                _isTeacher = AuthenticationCache.RoleId == 2;
            }, cancellationToken);

            if (!string.IsNullOrWhiteSpace(SearchText))
                UpdateSuggestions();
        }

        /// <summary>
        /// Triggered when the current user changes, updating the IsTeacher property accordingly.
        /// </summary>
        /// <param name="user"></param>
        private void OnUserChanged(User? user)
        {
            IsTeacher = AuthenticationCache.RoleId == 2;
        }
    }
}
