using LaboratoryApp.Domain.DTOs.Content.Assessment;
using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels
{
    public class CreateNewExerciseViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserProvider _userProvider;
        private readonly ICounterService _counterService;
        private readonly IDialogService _dialogService;
        private readonly IExerciseService _exerciseService;

        private bool _canGoNext = true;
        private bool _canGoBack = false;
        private BaseViewModel _currentView;

        public InformationViewModel Step1Vm { get; }
        public ContentViewModel Step2Vm { get; }
        public SettingViewModel Step3Vm { get; }

        #region Commands
        public ICommand SaveCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }
        public ICommand NavigateToNextPage { get; set; }
        public ICommand NavigateToPreviousPage { get; set; }
        #endregion

        #region Properties
        public bool CanGoNext
        {
            get => _canGoNext;
            set
            {
                _canGoNext = value;
                OnPropertyChanged();
            }
        }
        public bool CanGoBack
        {
            get => _canGoBack;
            set
            {
                _canGoBack = value;
                OnPropertyChanged();
            }
        }
        public BaseViewModel CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public ExerciseSummaryDTO NewExercise { get; set; }
        #endregion

        public CreateNewExerciseViewModel(IServiceProvider serviceProvider,
                                          IUserProvider userProvider,
                                          ICounterService counterService,
                                          IDialogService dialogService,
                                          IExerciseService exerciseService)
        {
            _serviceProvider = serviceProvider;
            _userProvider = userProvider;
            _counterService = counterService;
            _dialogService = dialogService;
            _exerciseService = exerciseService;

            Step1Vm = _serviceProvider.GetRequiredService<InformationViewModel>();
            Step2Vm = _serviceProvider.GetRequiredService<ContentViewModel>();
            Step3Vm = _serviceProvider.GetRequiredService<SettingViewModel>();

            CurrentView = Step1Vm;

            SaveCommand = new RelayCommand<object>(p => true, async p =>
            {
                await ExecuteSave();
                if (p is CreateNewExerciseWindow window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            });
            WindowClosingCommand = new RelayCommand<object>(p => true, async p => await ExecuteWindowClosing());
            NavigateToNextPage = new RelayCommand<object>(p => CanGoNext, p => GoNext());
            NavigateToPreviousPage = new RelayCommand<object>(p => CanGoBack, p => GoBack());
        }

        /// <summary>
        /// Xử lý chuyển sang bước tiếp theo
        /// </summary>
        private void GoNext()
        {
            if (CurrentView == Step1Vm)
            {
                CurrentView = Step2Vm;
                CanGoBack = true;
            }
            else if (CurrentView == Step2Vm)
            {
                CurrentView = Step3Vm;
                CanGoNext = false;
            }
        }

        /// <summary>
        /// Xử lý chuyển về bước trước đó
        /// </summary>
        private void GoBack()
        {
            if (CurrentView == Step3Vm)
            {
                CurrentView = Step2Vm;
                CanGoNext = true;
            }
            else if (CurrentView == Step2Vm)
            {
                CurrentView = Step1Vm;
                CanGoBack = false;
            }
        }

        private async Task ExecuteWindowClosing()
        {
            await Step2Vm.OnCleanupAsync();
        }

        private async Task<bool> ExecuteSave()
        {
            var newExercise = new Exercise();

            // Lưu dữ liệu từ các bước vào đối tượng Exercise
            await Step2Vm.OnSaveAsync();

            // Bước 1: Thông tin tác giả
            newExercise.AuthorId = AuthenticationCache.CurrentAuthentication?.User.Id ?? 0;
            newExercise.Author = await _userProvider.GetUserByIdAsync(newExercise.AuthorId) ?? new User();

            // Bước 2: Thông tin chung
            newExercise.Title = Step1Vm.Title;
            newExercise.Description = Step1Vm.Description;
            newExercise.DurationInMinutes = Step1Vm.DurationInMinutes;
            if (double.TryParse(Step1Vm.PassScore, out double passScore))
            {
                newExercise.PassScore = passScore;
            }
            else
            {
                _dialogService.ShowMessage("Điểm đạt phải là một số hợp lệ. (Ex: 5 hoặc 4.5)", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Bước 3: Cài đặt
            newExercise.MaxAttempts = Step3Vm.MaxAttempts;
            newExercise.ShuffleQuestions = Step3Vm.ShuffleQuestions;
            newExercise.ShuffleAnswers = Step3Vm.ShuffleAnswers;
            newExercise.ShowResultImmediately = Step3Vm.ShowResultImmediately;
            newExercise.ShowCorrectAnswers = Step3Vm.ShowCorrectAnswers;
            newExercise.AllowLateSubmission = Step3Vm.AllowLateSubmission;
            newExercise.LateSubmissionPenalty = Step3Vm.LateSubmissionPenalty;

            // Bước 4: Nội dung bài tập
            newExercise.Questions = Step2Vm.Result.Questions;
            newExercise.QuestionBlocks = Step2Vm.Result.QuestionBlocks;

            await _exerciseService.SaveNewExerciseAsync(newExercise);

            NewExercise = new ExerciseSummaryDTO
            {
                ExerciseId = newExercise.Id,
                Title = newExercise.Title,
                Status = newExercise.Status
            };
            return true;
        }
    }
}
