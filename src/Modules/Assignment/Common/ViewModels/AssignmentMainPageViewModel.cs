using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;

using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using LaboratoryApp.src.Core.Models.Authentication.Enums;

using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Assignment.Common.Views;
using LaboratoryApp.src.Modules.Assignment.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Assignment.ExerciseFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Assignment.Common.Views;

using LaboratoryApp.src.Services.Assignment;

using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Data.Providers.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;

namespace LaboratoryApp.src.Modules.Assignment.Common.ViewModels
{
    public class AssignmentMainPageViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthorizationCache _authorizationCache;
        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentCache _assignmentCache;
        private readonly Func<INavigationService, IServiceProvider, IAuthorizationCache, IAssignmentService, IAssignmentCache, ExerciseSet, ExerciseManagerViewModel> _exerciseManagerVmFactory;

        private bool _isTeacher;

        private ObservableCollection<ExerciseSet> _exerciseSets;

        #region Commands
        public ICommand AddNewSetCommand { get; set; }
        public ICommand InsertExerciseSet { get; set; }
        public ICommand OpenExerciseSetCommand { get; set; }
        #endregion

        #region Properties
        public bool IsTeacher
        {
            get => _isTeacher;
            set
            {
                _isTeacher = value;
                OnPropertyChanged(nameof(IsTeacher));
            }
        }
        public ObservableCollection<ExerciseSet> ExerciseSets
        {
            get => _exerciseSets;
            set
            {
                _exerciseSets = value;
                OnPropertyChanged(nameof(ExerciseSets));
            }
        }
        #endregion

        public AssignmentMainPageViewModel(INavigationService navigationService,
                                           IServiceProvider serviceProvider,
                                           IAuthorizationCache authorizationCache,
                                           IAssignmentService assignmentService,
                                           IAssignmentCache assignmentCache,
                                           Func<INavigationService, IServiceProvider, IAuthorizationCache, IAssignmentService, IAssignmentCache, ExerciseSet, ExerciseManagerViewModel> exerciseManagerVmFactory)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _authorizationCache = authorizationCache;
            _assignmentService = assignmentService;
            _assignmentCache = assignmentCache;
            _exerciseManagerVmFactory = exerciseManagerVmFactory;

            ExerciseSets = new ObservableCollection<ExerciseSet>();

            AuthenticationCache.CurrentUserChanged += OnUserChanged;


            #region Commands
            AddNewSetCommand = new RelayCommand<object>(p => true, (p) =>
            {
                // Thêm mới bài tập
                var window = _serviceProvider.GetRequiredService<AddExerciseSetWindow>();
                window.ShowDialog();

                ExerciseSets = new ObservableCollection<ExerciseSet>(_assignmentService.GetAllExerciseSetsByUserId(AuthenticationCache.CurrentUser?.Id ?? 0));
            });

            InsertExerciseSet = new RelayCommand<object>((p) => true, (p) =>
            {
                if (AuthenticationCache.IsAuthenticated == false)
                {
                    MessageBox.Show("Vui lòng đăng nhập để có thể nhập bộ bài tập mới", "Yêu cầu đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var window = _serviceProvider.GetRequiredService<InsertExerciseSetWindow>();
                window.ShowDialog();

                ExerciseSets = new ObservableCollection<ExerciseSet>(_assignmentService.GetAllExerciseSetsByUserId(AuthenticationCache.CurrentUser?.Id ?? 0));
            });

            OpenExerciseSetCommand = new RelayCommand<object>(p => true, (p) =>
            {
                var selectedSet = (ExerciseSet)p;
                var page = _serviceProvider.GetRequiredService<ExerciseManagerPage>();
                page.DataContext = _exerciseManagerVmFactory(_navigationService, _serviceProvider, _authorizationCache, _assignmentService, _assignmentCache, selectedSet);
                _navigationService.NavigateTo(page);
            });
            #endregion
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Khởi tạo dữ liệu bất đồng bộ ở đây
                _isTeacher = AuthenticationCache.CurrentUser?.Role.HasFlag(Role.Instructor) ?? false;
                ExerciseSets = new ObservableCollection<ExerciseSet>(_assignmentService.GetAllExerciseSetsByUserId(AuthenticationCache.CurrentUser?.Id ?? 0));
            }, cancellationToken);
        }

        private void OnUserChanged(UserDTO? user)
        {
            IsTeacher = AuthenticationCache.CurrentUser?.Role.HasFlag(Role.Instructor) ?? false;
            ExerciseSets = new ObservableCollection<ExerciseSet>(_assignmentService.GetAllExerciseSetsByUserId(AuthenticationCache.CurrentUser?.Id ?? 0));
        }

        private void Dispose()
        {
            AuthenticationCache.CurrentUserChanged -= OnUserChanged;
        }
    }
}
