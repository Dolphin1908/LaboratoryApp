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
        bool InsertNewExerciseSet(string code, string password);
        void SaveNewExerciseSet(ExerciseSet set);
        List<ExerciseSet> GetAllExerciseSetsByUserId(long userId);
        #endregion

        #region Exercise
        void SaveNewExercise(ExerciseSet set, Exercise exercise);
        List<Exercise> GetAllExercisesBySetId(long setId);
        #endregion
    }
}
