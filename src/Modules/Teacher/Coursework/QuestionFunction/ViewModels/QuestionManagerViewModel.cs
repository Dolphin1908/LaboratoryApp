using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Authorization;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Coursework.QuestionFunction.Views;
using LaboratoryApp.src.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Coursework.QuestionFunction.ViewModels
{
    public class QuestionManagerViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthorizationCache _authorizationCache;
        private readonly INavigationService _navigationService;

        private bool _isOwner;

        private long _setId;
        private Exercise _selectedExercise;
        private List<Question> _questions;

        #region Commands
        public ICommand AddMultipleQuestionsCommand { get; set; } // Add multiple new questions at once (in the future)
        public ICommand AddQuestionCommand { get; set; } // Add a single new question
        public ICommand DeleteMultipleQuestionsCommand { get; set; } // Delete multiple selected questions
        public ICommand DeleteQuestionCommand { get; set; } // Delete selected question
        public ICommand EditQuestionCommand { get; set; } // Edit selected question
        #endregion

        #region Properties
        public bool IsOwner
        {
            get => _isOwner;
            set
            {
                _isOwner = value;
                OnPropertyChanged(nameof(IsOwner));
            }
        }
        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set
            {
                _selectedExercise = value;
                OnPropertyChanged(nameof(SelectedExercise));
            }
        }
        public List<Question> Questions
        {
            get => _questions;
            set
            {
                _questions = value;
                OnPropertyChanged(nameof(Questions));
            }
        }
        #endregion

        public QuestionManagerViewModel(IServiceProvider serviceProvider,
                                        IAuthorizationCache authorizationCache,
                                        INavigationService navigationService,
                                        long setId,
                                        Exercise selectedExercise)
        {
            _serviceProvider = serviceProvider;
            _authorizationCache = authorizationCache;
            _navigationService = navigationService;
            _setId = setId;
            _selectedExercise = selectedExercise;

            _questions = new List<Question>();

            AuthenticationCache.CurrentAuthenticationChanged += OnUserChanged;

            LoadData();
            InitializePermissions();

            #region Commands
            AddMultipleQuestionsCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Do nothing
            });

            AddQuestionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Điều hướng tới cửa sổ nhập câu hỏi mới
                var window = _serviceProvider.GetRequiredService<AddQuestionWindow>();
                window.ShowDialog();
                // Chuyển dữ liệu của exercise cho viewmodel xử lý
                // Thêm câu hỏi mới vào database và cache, đồng thời lưu Id vào QuestionIds của Exercise
            });

            DeleteMultipleQuestionsCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Do nothing
            });

            DeleteQuestionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Do nothing
            });

            EditQuestionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Do nothing
            });
            #endregion
        }

        private void InitializePermissions()
        {

        }

        private void OnUserChanged(AuthenticationResponseDTO? user)
        {
            var currAccess = _authorizationCache.AllExerciseSetAccess.FirstOrDefault(esa => esa.UserId == (AuthenticationCache.CurrentAuthentication?.User.Id ?? 0) && esa.ExerciseSetId == _setId);
            if (currAccess == null)
            {
                var backPage = _serviceProvider.GetRequiredService<ExerciseSetManagerPage>();
                _navigationService.NavigateTo(backPage);
            }
        }

        public void LoadData()
        {
            // Do nothing
        }

        private void Dispose()
        {
            AuthenticationCache.CurrentAuthenticationChanged -= OnUserChanged;
        }
    }
}
