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
    public class ReadingQuestionViewModel : QuestionBaseViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;

        public int Score { get; set; } = 1;
        public ObservableCollection<string> Attachments { get; set; } = new();

        public ObservableCollection<string> Passages { get; set; } = new();
        public ObservableCollection<string>? ImageUrls { get; set; } = new();
        public ObservableCollection<Question> SubQuestions { get; set; } = new();

        public ICommand AddQuestionCommand {  get; set; }

        public ReadingQuestionViewModel()
        {
            AddQuestionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Do nothing now
            });
        }

        public override Question BuildQuestion()
        {
            return new ReadingQuestion
            {
                Text = Text,
                Explanation = Explanation,

                Score = Score,
                Type = QuestionType.Reading,
                Attachments = Attachments?.ToList() ?? new(),

                Passages = Passages?.ToList() ?? new(),
                ImageUrls = ImageUrls?.ToList() ?? new(),
                SubQuestions = SubQuestions?.ToList() ?? new()
            };
        }
    }
}
