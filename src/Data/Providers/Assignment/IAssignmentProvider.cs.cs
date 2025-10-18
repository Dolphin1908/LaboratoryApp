using LaboratoryApp.src.Core.Models.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Assignment
{
    public interface IAssignmentProvider
    {
        #region ExerciseSet
        Task<ExerciseSet?> GetExerciseSetById(long setId);
        Task<List<ExerciseSet>> GetAllExerciseSets();
        Task CreateNewExerciseSet(ExerciseSet set);
        Task UpdateExerciseSet(ExerciseSet set);
        Task DeleteExerciseSet(ExerciseSet set);
        #endregion

        #region Exercise
        Task<List<Exercise>> GetAllExercises();
        Task CreateNewExercise(Exercise exercise);
        #endregion
    }
}
