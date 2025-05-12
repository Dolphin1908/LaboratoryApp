using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.Models.English.LectureFunction.Enums;

namespace LaboratoryApp.Models.English.LectureFunction
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
