using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Data.Providers.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Caches.Assignment
{
    public interface IAssignmentCache
    {
        List<ExerciseSet> AllExerciseSets { get; set; }
        void LoadAllData(IAssignmentProvider provider);
    }
}
