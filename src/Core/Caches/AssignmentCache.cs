using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Services.Assignment;

namespace LaboratoryApp.src.Core.Caches
{
    public static class AssignmentCache
    {
        private static readonly object _lock = new();

        public static List<ExerciseSet> AllExerciseSets { get; set; } = new();

        public static void LoadAllData(IAssignmentService service)
        {
            lock (_lock)
            {
                AllExerciseSets = service.GetAllExerciseSets();
            }
        }
    }
}
