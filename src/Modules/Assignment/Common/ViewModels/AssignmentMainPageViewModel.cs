using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Core.Models.Authentication.DTOs;
using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Teacher.Assignment.Common.Views;
using LaboratoryApp.src.Modules.Teacher.Assignment.Common.ViewModels;

using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Data.Providers.Assignment;
using LaboratoryApp.src.Data.Providers.Authorization;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Services.Assignment;
using LaboratoryApp.src.Modules.Assignment.Exercise.Views;
using LaboratoryApp.src.Modules.Assignment.Exercise.ViewModels;

namespace LaboratoryApp.src.Modules.Assignment.Common.ViewModels
{
    public class AssignmentMainPageViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAssignmentService _assignmentService;
        private readonly Func<ExerciseSet, ExerciseManagerViewModel> _exerciseManagerVmFactory;

        private bool _isTeacher;

        private ObservableCollection<ExerciseSet> _exerciseSets;

        #region Commands
        public ICommand AddNewSetCommand { get; set; }
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

        public AssignmentMainPageViewModel(IServiceProvider serviceProvider,
                                           IAssignmentService assignmentService,
                                           Func<ExerciseSet, ExerciseManagerViewModel> exerciseManagerVmFactory)
        {
            _serviceProvider = serviceProvider;
            _assignmentService = assignmentService;
            _exerciseManagerVmFactory = exerciseManagerVmFactory;

            ExerciseSets = new ObservableCollection<ExerciseSet>();

            AuthenticationCache.CurrentUserChanged += OnUserChanged;


            #region Commands
            AddNewSetCommand = new RelayCommand<object>(p => true, (p) =>
            {
                // Thêm mới bài tập
                var window = _serviceProvider.GetRequiredService<AddExerciseSetWindow>();
                if (window.DataContext is ExerciseSetViewModel vm && vm is IAsyncInitializable init)
                {
                    // Khởi tạo dữ liệu bất đồng bộ
                    _ = init.InitializeAsync();
                }
                window.ShowDialog();
            });

            OpenExerciseSetCommand = new RelayCommand<object>(p => true, (p) =>
            {
                var selectedSet = (ExerciseSet)p;
                var window = _serviceProvider.GetRequiredService<ExerciseManagerWindow>();
                window.DataContext = _exerciseManagerVmFactory(selectedSet);
                window.ShowDialog();
            });
            #endregion
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Khởi tạo dữ liệu bất đồng bộ ở đây
                _isTeacher = AuthenticationCache.RoleId == 2;
                ExerciseSets = new ObservableCollection<ExerciseSet>(_assignmentService.GetAllExerciseSetsByUserId(AuthenticationCache.CurrentUser?.Id ?? 0));
            }, cancellationToken);
        }

        private void OnUserChanged(UserDTO? user)
        {
            IsTeacher = AuthenticationCache.RoleId == 2;
            ExerciseSets = new ObservableCollection<ExerciseSet>(_assignmentService.GetAllExerciseSetsByUserId(AuthenticationCache.CurrentUser?.Id ?? 0));
        }

        public void Dispose()
        {
            AuthenticationCache.CurrentUserChanged -= OnUserChanged;
        }
    }
}
