using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English.Lecture
{
    public abstract class QuestionModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Explanation { get; set; }
    }

    public class MultipleChoiceQuestionModel : QuestionModel
    {
        public List<OptionModel> Options { get; set; }
    }

    public class FillInTheBlankQuestionModel : QuestionModel
    {
        public string CorrectAnswer { get; set; }
    }

    public class TrueFalseQuestionModel : QuestionModel
    {
        public bool IsTrue { get; set; }
    }
}
