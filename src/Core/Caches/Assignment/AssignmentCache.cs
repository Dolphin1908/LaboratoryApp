using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Data.Providers.Assignment;

namespace LaboratoryApp.src.Core.Caches.Assignment
{
    public class AssignmentCache : IAssignmentCache
    {
        private readonly object _lock = new();

        public List<ExerciseSet> AllExerciseSets { get; set; } = new();

        public void LoadAllData(IAssignmentProvider assignmentProvider)
        {
            lock (_lock)
            {
                AllExerciseSets = assignmentProvider.GetAllExerciseSets().GetAwaiter().GetResult();
            }
        }
    }
}