using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Helpers;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Services.Assignment.QuestionFunction;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.QuestionFunction.ViewModels
{
    public class QuestionViewModel : BaseViewModel
    {
        private readonly IQuestionService _questionService;

        private QuestionBaseViewModel? _currentQuestionViewModel;

        #region Commands
        public ICommand AddQuestionCommand { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<SelectableEnumDisplay<QuestionType>> QuestionTypeOptions { get; }
        public QuestionType SelectedQuestionType { get; set; }
        public QuestionBaseViewModel? CurrentQuestionViewModel
        {
            get => _currentQuestionViewModel;
            set
            {
                _currentQuestionViewModel = value;
                OnPropertyChanged(nameof(CurrentQuestionViewModel));
            }
        }
        #endregion

        public QuestionViewModel(IQuestionService questionService)
        {
            _questionService = questionService;

            QuestionTypeOptions = new ObservableCollection<SelectableEnumDisplay<QuestionType>>(
                Enum.GetValues(typeof(QuestionType))
                    .Cast<QuestionType>()
                    .Select(qt => new SelectableEnumDisplay<QuestionType>(qt))
            );

            AddQuestionCommand = new RelayCommand<object>((p) => true, (p) => AddQuestion());
        }

        private void AddQuestion()
        {
            CurrentQuestionViewModel = _questionService.Create(SelectedQuestionType);
        }
    }
}
