using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.Domain.Models.Content;

namespace LaboratoryApp.src.Services.Assignment.ExerciseFunction
{
    public interface IExerciseService
    {
        void SaveNewExercise(ExerciseSet set, Exercise exercise);
        List<Exercise> GetAllExercisesBySetId(long setId);
    }
}
