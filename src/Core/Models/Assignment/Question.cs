using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Assignment.Enums;

namespace LaboratoryApp.src.Core.Models.Assignment
{
    public abstract class Question
    {
        public long Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;

        public int Score { get; set; } = 1; // Default score for the question
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy;
        public List<string>? Attachments { get; set; } = new(); // URLs or file paths to attachments
    }

    // Multiple choice question
    public class MultipleChoiceQuestion : Question
    {
        public List<Option> Options { get; set; } = new();
        public bool IsMultipleAnswer { get; set; } = false; // True if multiple answers are correct
        public bool ShuffleOptions { get; set; } = true; // True to shuffle options each time
    }

    // Fill in the blank question
    public class FillInTheBlankQuestion : Question
    {
        public List<string> CorrectAnswer { get; set; } = new();
        public bool CaseSensitive { get; set; } = false; // True if the answer
    }

    // Writing question
    public class WritingQuestion : Question
    {
        public string SampleAnswer { get; set; } = string.Empty;
        public int WordLimit { get; set; } = 200; // Suggested word limit
    }

    // Reading question
    public class ReadingQuestion : Question
    {
        public List <string> Passages { get; set; } = new();
        public List <string>? ImageUrls { get; set; } = new();
        public List<Question> SubQuestions { get; set; } = new();
    }

    // Listening question
    public class ListeningQuestion : Question
    {
        public string AudioFilePath { get; set; } = string.Empty;
        public string Transcript { get; set; } = string.Empty;
        public List<Question> SubQuestions { get; set; } = new();
    }
}
