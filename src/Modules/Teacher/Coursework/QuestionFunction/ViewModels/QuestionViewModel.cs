using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Helpers;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.src.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Coursework.QuestionFunction.ViewModels
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

        public QuestionBaseViewModel Create(QuestionType type)
        {
            return type switch
            {
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private void AddQuestion()
        {
            if (SelectedQuestionType == QuestionType.None)
                return;

            CurrentQuestionViewModel = Create(SelectedQuestionType);
        }
    }
}
