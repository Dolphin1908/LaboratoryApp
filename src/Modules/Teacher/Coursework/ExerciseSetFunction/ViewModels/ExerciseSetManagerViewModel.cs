using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Assignment;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.Interfaces;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.Views;
using LaboratoryApp.src.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.ViewModels
{
    public class ExerciseSetManagerViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;
        private readonly IAssignmentCache _assignmentCache;
        private readonly IAuthorizationCache _authorizationCache;
        private readonly IExerciseService _exerciseService;
        private readonly IExerciseSetService _exerciseSetService;
        private readonly Func<INavigationService, IServiceProvider, IAssignmentCache, IAuthorizationCache, IExerciseService, ExerciseSet, ExerciseManagerViewModel> _exerciseManagerVmFactory;

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

        public ExerciseSetManagerViewModel(INavigationService navigationService,
                                           IServiceProvider serviceProvider,
                                           IDialogService dialogService,
                                           IAssignmentCache assignmentCache,
                                           IAuthorizationCache authorizationCache,
                                           IExerciseService exerciseService,
                                           IExerciseSetService exerciseSetService,
                                           Func<INavigationService, IServiceProvider, IAssignmentCache, IAuthorizationCache, IExerciseService, ExerciseSet, ExerciseManagerViewModel> exerciseManagerVmFactory)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;
            _assignmentCache = assignmentCache;
            _authorizationCache = authorizationCache;
            _exerciseService = exerciseService;
            _exerciseSetService = exerciseSetService;
            _exerciseManagerVmFactory = exerciseManagerVmFactory;

            _exerciseSets = new ObservableCollection<ExerciseSet>();

            AuthenticationCache.CurrentAuthenticationChanged += OnUserChanged;

            #region Commands
            AddNewSetCommand = new RelayCommand<object>(p => true, (p) =>
            {
                // Thêm mới bài tập
                var window = _serviceProvider.GetRequiredService<AddExerciseSetWindow>();
                window.ShowDialog();

                LoadData();
            });

            InsertExerciseSet = new RelayCommand<object>((p) => true, (p) =>
            {
                if (AuthenticationCache.IsAuthenticated == false)
                {
                    _dialogService.ShowMessage("Vui lòng đăng nhập để có thể nhập bộ bài tập mới", "Yêu cầu đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var window = _serviceProvider.GetRequiredService<InsertExerciseSetWindow>();
                window.ShowDialog();

                LoadData();
            });

            OpenExerciseSetCommand = new RelayCommand<object>(p => true, (p) =>
            {
                var selectedSet = (ExerciseSet)p;
                var page = _serviceProvider.GetRequiredService<ExerciseManagerPage>();
                page.DataContext = _exerciseManagerVmFactory(_navigationService, _serviceProvider, _assignmentCache, _authorizationCache, _exerciseService, selectedSet);
                _navigationService.NavigateTo(page);
            });
            #endregion
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Khởi tạo dữ liệu bất đồng bộ ở đây
                //_isTeacher = AuthenticationCache.CurrentUser?.Role.HasFlag(Role.Instructor) ?? false;
                LoadData();
            }, cancellationToken);
        }

        private void OnUserChanged(AuthenticationResponseDTO? user)
        {
            //IsTeacher = AuthenticationCache.CurrentUser?.Role.HasFlag(Role.Instructor) ?? false;
            LoadData();
        }

        private void LoadData()
        {
            ExerciseSets = new ObservableCollection<ExerciseSet>(_exerciseSetService.GetAllExerciseSetsByUserId(AuthenticationCache.CurrentAuthentication?.User.Id ?? 0));
        }

        private void Dispose()
        {
            AuthenticationCache.CurrentAuthenticationChanged -= OnUserChanged;
        }
    }
}
