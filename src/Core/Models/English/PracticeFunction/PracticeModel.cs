using LaboratoryApp.src.Core.Models.English.PracticeFunction.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.PracticeFunction
{
    public class PracticeModel
    {
        #region Infomation
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        #endregion

        #region Settings
        public string Code { get; set; } // Unique code for sharing
        public PracticeType Type { get; set; } // e.g., Vocabulary, Grammar, Listening
        public bool IsPublic { get; set; } // Public or Private
        #endregion

        #region Content
        public List<ExerciseModel> Exercises { get; set; }
        #endregion
    }
}
