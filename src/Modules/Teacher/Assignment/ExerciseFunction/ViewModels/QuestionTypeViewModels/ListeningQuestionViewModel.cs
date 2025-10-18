using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Assignment.Enums;
using LaboratoryApp.src.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels.QuestionTypeViewModels
{
    public class ListeningQuestionViewModel : QuestionBaseViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;

        public int Score { get; set; } = 1;
        public ObservableCollection<string> Attachments { get; set; } = new();

        public string AudioFilePath { get; set; } = string.Empty;
        public string Transcript { get; set; } = string.Empty;
        public ObservableCollection<Question> SubQuestions { get; set; } = new();

        public ICommand AddQuestionCommand { get; set; }

        public ListeningQuestionViewModel()
        {
            AddQuestionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Do nothing now
            });
        }

        public override Question BuildQuestion()
        {
            return new ListeningQuestion
            {
                Text = Text,
                Explanation = Explanation,

                Score = Score,
                Type = QuestionType.Listening,
                Attachments = Attachments?.ToList() ?? new(),

                AudioFilePath = AudioFilePath,
                Transcript = Transcript,
                SubQuestions = SubQuestions?.ToList() ?? new()
            };
        }
    }
}
