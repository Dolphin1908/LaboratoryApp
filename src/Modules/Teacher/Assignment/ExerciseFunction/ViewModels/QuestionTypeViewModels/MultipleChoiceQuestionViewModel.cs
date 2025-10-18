using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Assignment.Enums;
using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels.QuestionTypeViewModels
{
    public class MultipleChoiceQuestionViewModel : QuestionBaseViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;

        public int Score { get; set; } = 1;
        public ObservableCollection<string> Attachments { get; set; } = new();

        public ObservableCollection<Option> Options { get; set; } = new();
        public bool IsMultipleAnswer { get; set; }
        public bool ShuffleOptions { get; set; }

        public ICommand AddOptionCommand { get; set; }

        public MultipleChoiceQuestionViewModel()
        {
            AddOptionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Do nothing now
            });
        }

        public override Question BuildQuestion()
        {
            return new MultipleChoiceQuestion
            {
                Text = Text,
                Explanation = Explanation,

                Score = Score,
                Type = QuestionType.MultipleChoice,
                Attachments = Attachments?.ToList() ?? new(),
                
                Options = Options?.ToList() ?? new(),
                IsMultipleAnswer = IsMultipleAnswer,
                ShuffleOptions = ShuffleOptions
            };
        }
    }
}
