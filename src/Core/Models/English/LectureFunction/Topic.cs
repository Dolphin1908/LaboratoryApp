using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.ExerciseFunction;

namespace LaboratoryApp.src.Core.Models.English.LectureFunction
{
    public class Topic
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentHTML { get; set; }

        public long? ParentTopicId { get; set; }
        public List<Topic> SubTopics { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
