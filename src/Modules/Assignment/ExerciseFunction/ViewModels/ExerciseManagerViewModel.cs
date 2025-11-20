using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.Enums.Authorization;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Assignment.ExerciseSetFunction.Views;
using LaboratoryApp.src.Modules.Assignment.QuestionFunction.ViewModels;
using LaboratoryApp.src.Modules.Assignment.QuestionFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.Views;
using LaboratoryApp.src.Services.Assignment.ExerciseFunction;
using LaboratoryApp.src.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Assignment.ExerciseFunction.ViewModels
{
    public class ExerciseManagerViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAssignmentCache _assignmentCache;
        private readonly IAuthorizationCache _authorizationCache;
        private readonly IExerciseService _exerciseService;

        private bool _isModifier;
        private bool _isOwner;
        private ExerciseSet _selectedExerciseSet;
        private ObservableCollection<Exercise> _exercises;

        private Func<IExerciseService, ExerciseSet, ExerciseViewModel> _addExerciseVmFactory;
        private Func<IServiceProvider, IAuthorizationCache, INavigationService, long, Exercise, QuestionManagerViewModel> _exerciseDetailVmFactory;

        #region Commands
        public ICommand AddExerciseCommand { get; set; }
        public ICommand OpenExerciseCommand { get; set; }
        #endregion

        #region Properties
        public bool IsModifier
        {
            get => _isModifier;
            set
            {
                _isModifier = value;
                OnPropertyChanged(nameof(IsModifier));
            }
        }
        public bool IsOwner
        {
            get => _isOwner;
            set
            {
                _isOwner = value;
                OnPropertyChanged(nameof(IsOwner));
            }
        }
        public ExerciseSet SelectedExerciseSet
        {
            get => _selectedExerciseSet;
            set
            {
                _selectedExerciseSet = value;
                OnPropertyChanged(nameof(SelectedExerciseSet));
            }
        }
        public ObservableCollection<Exercise> Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged(nameof(Exercises));
            }
        }
        #endregion

        public ExerciseManagerViewModel(INavigationService navigationService,
                                        IServiceProvider serviceProvider,
                                        IAssignmentCache assignmentCache,
                                        IAuthorizationCache authorizationCache,
                                        IExerciseService exerciseService,
                                        ExerciseSet selectedExerciseSet,
                                        Func<IExerciseService, ExerciseSet, ExerciseViewModel> addExerciseVmFactory,
                                        Func<IServiceProvider, IAuthorizationCache, INavigationService, long, Exercise, QuestionManagerViewModel> exerciseDetailVmFactory)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _assignmentCache = assignmentCache;
            _authorizationCache = authorizationCache;
            _exerciseService = exerciseService;

            _selectedExerciseSet = selectedExerciseSet;
            _exercises = new ObservableCollection<Exercise>();
            _addExerciseVmFactory = addExerciseVmFactory;
            _exerciseDetailVmFactory = exerciseDetailVmFactory;

            AuthenticationCache.CurrentAuthenticationChanged += OnUserChanged;

            LoadData();
            InitializePermissions();

            #region Commands 
            AddExerciseCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<AddExerciseWindow>();
                window.DataContext = _addExerciseVmFactory(_exerciseService, selectedExerciseSet);
                window.ShowDialog();

                LoadData();
            });

            OpenExerciseCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var selectedExercise = (Exercise)p;
                var page = _serviceProvider.GetRequiredService<QuestionManagerPage>();
                page.DataContext = _exerciseDetailVmFactory(_serviceProvider, _authorizationCache, _navigationService, _selectedExerciseSet.Id, selectedExercise);
                _navigationService.NavigateTo(page);
            });
            #endregion
        }

        private void InitializePermissions()
        {
            var currAccess = _authorizationCache.AllExerciseSetAccess.FirstOrDefault(esa => esa.UserId == (AuthenticationCache.CurrentAuthentication?.User.Id ?? 0) && esa.ExerciseSetId == _selectedExerciseSet.Id);
            IsOwner = currAccess != null && currAccess.Level.HasFlag(AccessLevel.Owner);
            IsModifier = currAccess != null && currAccess.Level.HasFlag(AccessLevel.Edit);
        }

        private void LoadData()
        {
            Exercises = new ObservableCollection<Exercise>(_exerciseService.GetAllExercisesBySetId(_selectedExerciseSet.Id));
        }

        private void OnUserChanged(AuthenticationResponseDTO? user)
        {
            var currAccess = _authorizationCache.AllExerciseSetAccess.FirstOrDefault(esa => esa.UserId == (AuthenticationCache.CurrentAuthentication?.User.Id ?? 0) && esa.ExerciseSetId == _selectedExerciseSet.Id);
            if (currAccess == null)
            {
                var backPage = _serviceProvider.GetRequiredService<ExerciseSetManagerPage>();
                _navigationService.NavigateTo(backPage);
            }
        }

        private void Dispose()
        {
            AuthenticationCache.CurrentAuthenticationChanged -= OnUserChanged;
        }
    }
}
