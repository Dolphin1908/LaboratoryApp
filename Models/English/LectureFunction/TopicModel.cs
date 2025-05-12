using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English.LectureFunction
{
    public class TopicModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentHTML { get; set; }

        public long? ParentTopicId { get; set; }
        public List<TopicModel> SubTopics { get; set; }
        public List<ExerciseModel> Exercises { get; set; }
    }
}
