using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authorization
{
    public class ExerciseAccess
    {
        public long Id { get; set; }
        public long ExerciseSetId { get; set; }

        public long? UserId { get; set; }
        public long? GroupId { get; set; }


        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    }
}
