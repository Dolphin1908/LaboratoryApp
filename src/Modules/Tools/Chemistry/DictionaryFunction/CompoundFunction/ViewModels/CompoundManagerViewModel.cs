using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.Enums.Users;
using LaboratoryApp.Domain.Interfaces.Services.Chemistry;
using LaboratoryApp.Domain.Models.Chemistry.CompoundFunction;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.CompoundFunction.ViewModels
{
    public class CompoundManagerViewModel : BaseViewModel, IAsyncInitializable, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;
        private readonly ICompoundService _compoundService;

        private string _searchText = string.Empty;
        private Compound _selectedCompound = null!;
        private bool _isTeacher;

        private ObservableCollection<Compound> _compounds;

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
        /// Construction
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="chemistryService"></param>
        /// <param name="chemistryDataCache"></param>
        public CompoundManagerViewModel(IServiceProvider serviceProvider,
                                        IDialogService dialogService,
                                        ICompoundService compoundService)
        {
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;
            _compoundService = compoundService;

            _compounds = new ObservableCollection<Compound>();

            AuthenticationCache.CurrentAuthenticationChanged += OnUserChanged;

            AddCompoundCommand = new RelayCommand<object>(p => true, p =>
            {
                _dialogService.ShowAddCompoundCommand(); // Gọi phương thức hiển thị cửa sổ thêm hợp chất mới qua dialog service tránh việc phụ thuộc trực tiếp vào View.
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
            Compounds.Clear();

            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var suggestions = _compoundService.GetSuggestions(SearchText, 10);
            foreach (var suggestion in suggestions)
            {
                Compounds.Add(suggestion);
            }
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
                IsTeacher = AuthenticationCache.CurrentAuthentication?.CurrentOrganizationProfile?.Role.HasFlag(UserRole.Instructor) ?? false;
            }, cancellationToken);

            if (!string.IsNullOrWhiteSpace(SearchText))
                UpdateSuggestions();
        }

        /// <summary>
        /// Triggered when the current user changes, updating the IsTeacher property accordingly.
        /// </summary>
        /// <param name="user"></param>
        private void OnUserChanged(AuthenticationResponseDTO? user)
        {
            IsTeacher = AuthenticationCache.CurrentAuthentication?.CurrentOrganizationProfile?.Role.HasFlag(UserRole.Instructor) ?? false;
        }

        public void Dispose()
        {
            AuthenticationCache.CurrentAuthenticationChanged -= OnUserChanged;
        }
    }
}
