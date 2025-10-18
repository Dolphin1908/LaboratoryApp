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
    public class FillInBlankQuestionViewModel : QuestionBaseViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;

        public int Score { get; set; } = 1;
        public ObservableCollection<string> Attachments { get; set; } = new();

        public ObservableCollection<string> CorrectAnswer { get; set; } = new();
        public bool CaseSensitive { get; set; } = false; // True if the answer

        public FillInBlankQuestionViewModel()
        {

        }

        public override Question BuildQuestion()
        {
            return new FillInTheBlankQuestion
            {
                Text = Text,
                Explanation = Explanation,

                Score = Score,
                Type = QuestionType.FillInBlank,
                Attachments = Attachments?.ToList() ?? new(),

                CorrectAnswer = CorrectAnswer?.ToList() ?? new(),
                CaseSensitive = CaseSensitive
            };
        }
    }
}
