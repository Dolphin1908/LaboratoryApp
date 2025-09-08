using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.PracticeFunction.Enums;

namespace LaboratoryApp.src.Core.Models.English.PracticeFunction
{
    public abstract class QuestionModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Explanation { get; set; }
        public abstract ExerciseType Type { get; }
    }

    // Multiple choice question
    public class MultipleChoiceQuestion : QuestionModel
    {
        public List<OptionModel> Options { get; set; } = new();
        public override ExerciseType Type => ExerciseType.MultipleChoice;
    }

    // Fill in the blank question
    public class FillInTheBlankQuestion : QuestionModel
    {
        public string CorrectAnswer { get; set; } = string.Empty;
        public override ExerciseType Type => ExerciseType.FillInBlank;
    }

    // Writing question
    public class WritingQuestion : QuestionModel
    {
        public string SampleAnswer { get; set; } = string.Empty;
        public override ExerciseType Type => ExerciseType.Writing;
    }

    // Reading question
    public class ReadingQuestion : QuestionModel
    {
        public List <string> Passages { get; set; } = new();
        public List <string>? ImageUrls { get; set; } = new();
        public List<QuestionModel> Questions { get; set; } = new();
        public override ExerciseType Type => ExerciseType.Reading;
    }

    // Listening question
    public class ListeningQuestion : QuestionModel
    {
        public string AudioFilePath { get; set; } = string.Empty;
        public List<QuestionModel> Questions { get; set; } = new();
        public override ExerciseType Type => ExerciseType.Listening;
    }
}
