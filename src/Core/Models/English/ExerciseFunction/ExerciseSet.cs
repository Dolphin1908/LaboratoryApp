using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.ExerciseFunction.Enums;

namespace LaboratoryApp.src.Core.Models.English.ExerciseFunction
{
    public class ExerciseSet
    {
        #region Infomation
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        #endregion

        #region Settings
        public string Code { get; set; } = string.Empty; // Unique code for sharing
        public string? Password { get; set; } // Optional password for access
        public ExerciseSetType Type { get; set; } // e.g., Vocabulary, Grammar, Listening
        public bool IsPublic { get; set; } = false; // Public or Private
        public bool RequireLogin { get; set; } = false; // Require login to access

        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy;// e.g., Easy, Medium, Hard
        #endregion

        #region Content
        public List<Exercise> Exercises { get; set; }
        #endregion
    }
}