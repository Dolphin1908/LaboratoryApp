using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using LaboratoryApp.src.Core.Models.Authorization.Enums;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.Views;
using LaboratoryApp.src.Services.Assignment;
using LaboratoryApp.src.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Assignment.ExerciseFunction.ViewModels
{
    public class ExerciseManagerViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthorizationCache _authorizationCache;
        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentCache _assignmentCache;

        private bool _isModifier;
        private bool _isOwner;
        private ExerciseSet _selectedExerciseSet;
        private List<Exercise> _exercises;

        private Func<IAssignmentService, ExerciseSet, ExerciseViewModel> _exerciseVmFactory;

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
        public List<Exercise> Exercises
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
                                        IAuthorizationCache authorizationCache,
                                        IAssignmentService assignmentService,
                                        IAssignmentCache assignmentCache,
                                        ExerciseSet selectedExerciseSet,
                                        Func<IAssignmentService, ExerciseSet, ExerciseViewModel> exerciseVmFactory)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _authorizationCache = authorizationCache;
            _assignmentService = assignmentService;
            _assignmentCache = assignmentCache;

            _selectedExerciseSet = selectedExerciseSet;
            _exerciseVmFactory = exerciseVmFactory;

            AuthenticationCache.CurrentUserChanged += OnUserChanged;

            LoadAllExerciseInSet();
            InitializePermissions();

            #region Commands 
            AddExerciseCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<AddExerciseWindow>();
                window.DataContext = _exerciseVmFactory(_assignmentService, selectedExerciseSet);
                window.ShowDialog();

                LoadAllExerciseInSet();
            });

            OpenExerciseCommand = new RelayCommand<object>((p) => true, (p) =>
            {

            });
            #endregion
        }

        private void InitializePermissions()
        {
            var currAccess = _authorizationCache.AllExerciseSetAccess.FirstOrDefault(esa => esa.UserId == (AuthenticationCache.CurrentUser?.Id ?? 0) && esa.ExerciseSetId == _selectedExerciseSet.Id);
            IsOwner = currAccess != null && currAccess.Level.HasFlag(AccessLevel.Owner);
            IsModifier = currAccess != null && currAccess.Level.HasFlag(AccessLevel.Edit);
        }

        private void LoadAllExerciseInSet()
        {
            Exercises = _assignmentService.GetAllExercisesBySetId(_selectedExerciseSet.Id);
        }

        private void OnUserChanged(UserDTO? user)
        {
            var currAccess = _authorizationCache.AllExerciseSetAccess.FirstOrDefault(esa => esa.UserId == (AuthenticationCache.CurrentUser?.Id ?? 0) && esa.ExerciseSetId == _selectedExerciseSet.Id);
            if (currAccess == null)
            {
                _navigationService.NavigateBack();
            }
            IsOwner = currAccess != null && currAccess.Level.HasFlag(AccessLevel.Owner);
            IsModifier = currAccess != null && currAccess.Level.HasFlag(AccessLevel.Edit);
        }

        private void Dispose()
        {
            AuthenticationCache.CurrentUserChanged -= OnUserChanged;
        }
    }
}
