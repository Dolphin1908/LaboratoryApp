using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Assignment.Enums;
using LaboratoryApp.src.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels.QuestionTypeViewModels
{
    public class WritingQuestionViewModel : QuestionBaseViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;

        public int Score { get; set; } = 1;
        public ObservableCollection<string> Attachments { get; set; } = new();

        public string SampleAnswer {  get; set; } = string.Empty;
        public int WordLimit { get; set; } = 200;

        public WritingQuestionViewModel()
        {

        }

        public override Question BuildQuestion()
        {
            return new WritingQuestion
            {
                Text = Text,
                Explanation = Explanation,

                Score = Score,
                Type = QuestionType.Writing,
                Attachments = Attachments?.ToList() ?? new(),

                SampleAnswer = SampleAnswer,
                WordLimit = WordLimit
            };
        }
    }
}
