using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.PracticeFunction.Enums;

namespace LaboratoryApp.src.Core.Models.English.PracticeFunction
{
    public class ExerciseModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ExerciseType Type { get; set; }
        public List<QuestionModel> Questions { get; set; }
    }
}
