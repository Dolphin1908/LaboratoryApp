using LaboratoryApp.src.Core.Models.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.Assignment
{
    public interface IAssignmentService
    {
        #region ExerciseSet
        void AddExerciseSet(ExerciseSet set);
        void UpdateExerciseSet(ExerciseSet set);
        List<ExerciseSet> GetAllExerciseSets();
        void DeleteExerciseSet(long id);
        #endregion

        #region Exercise

        #endregion
    }
}
