using LaboratoryApp.src.Core.Models.Authorization.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authorization
{
    public class ExerciseSetAccess
    {
        public ObjectId Id { get; }
        public long ExerciseSetId { get; set; }

        public long? UserId { get; set; }
        public long? GroupId { get; set; }

        public AccessLevel Level { get; set; } = AccessLevel.View; // e.g., View, Attempt, Grade, Edit

        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    }
}
