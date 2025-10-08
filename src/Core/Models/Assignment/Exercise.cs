using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Assignment.Enums;

namespace LaboratoryApp.src.Core.Models.Assignment
{
    public class Exercise
    {
        #region Infomation
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Instruction { get; set; } = string.Empty;
        #endregion

        #region Settings
        public string? Password { get; set; } // Optional password for access
        public ExerciseType Type { get; set; } // e.g., Quiz, Test, Practice
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy; // Overall difficulty level. E.g., Easy, Medium, Hard, Insane
        public List<TagType> Tags { get; set; } = new(); // Tags for categorization and search

        public int TimeLimitMinutes { get; set; } // Optional time limit for the exercise
        public bool ShuffleQuestions { get; set; } = true; // True to shuffle questions each time
        public bool ShowAnswersAfterCompletion { get; set; } = true; // Show correct answers after finishing
        #endregion

        #region Content
        public List<long> QuestionIds { get; set; }

        [NotMapped]
        public int Count => QuestionIds.Count; // Total number of questions
        #endregion
    }
}
