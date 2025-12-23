using LaboratoryApp.Domain.DTOs.Content.Assessment;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.DashboardFunction.ViewModels
{
    public class AssessmentMainPageViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDialogService _dialogService;
        private readonly IExerciseService _exerciseService;

        private ObservableCollection<ExerciseSummaryDTO> _allExercise;

        #region Commands
        public ICommand OpenCreateNewExerciseWindowCommand { get; set; }
        public ICommand OpenExerciseDetailWindowCommand { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<ExerciseSummaryDTO> AllExercise
        {
            get => _allExercise;
            set
            {
                _allExercise = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public AssessmentMainPageViewModel(IServiceProvider serviceProvider,
                                           IDialogService dialogService,
                                           IExerciseService exerciseService)
        {
            _serviceProvider = serviceProvider;

            _dialogService = dialogService;
            _exerciseService = exerciseService;

            LoadData();

            OpenCreateNewExerciseWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Logic to open the Create New Exercise view
                var subWindow = _serviceProvider.GetRequiredService<CreateNewExerciseWindow>();
                if (_dialogService.ShowDialogCenterOwner(subWindow) == true)
                {
                    var subViewModel = subWindow.DataContext as CreateNewExerciseViewModel;
                    AllExercise.Add(subViewModel!.NewExercise);
                }
            });
            OpenExerciseDetailWindowCommand = new RelayCommand<object>(p => true, async p =>
            {
                var exerciseSummary = p as ExerciseSummaryDTO;
                var window = _serviceProvider.GetRequiredService<ExerciseDetailWindow>();
                var viewModel = window.DataContext as ExerciseDetailViewModel;
                await viewModel!.LoadExerciseDetailAsync(exerciseSummary!.ExerciseId);

                _dialogService.ShowDialogCenterOwner(window);
            });
        }

        private async void LoadData()
        {
            AllExercise = new ObservableCollection<ExerciseSummaryDTO>(await _exerciseService.GetAllExerciseDashboardAsync(AuthenticationCache.CurrentAuthentication!.User.Id));
        }
    }
}
