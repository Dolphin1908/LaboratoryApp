using LaboratoryApp.Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.Assignment.ExerciseSetFunction
{
    public interface IExerciseSetService
    {
        bool InsertNewExerciseSet(string code, string password);
        void SaveNewExerciseSet(ExerciseSet set);
        List<ExerciseSet> GetAllExerciseSetsByUserId(long userId);
    }
}
