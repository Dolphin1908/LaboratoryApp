using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Data.Providers.Assignment.ExerciseFunction;
using LaboratoryApp.src.Data.Providers.Assignment.ExerciseSetFunction;
using LaboratoryApp.src.Data.Providers.Assignment.QuestionFunction;

namespace LaboratoryApp.src.Core.Caches.Assignment
{
    public interface IAssignmentCache
    {
        List<ExerciseSet> AllExerciseSets { get; set; }
        List<Exercise> AllExercises { get; set; }
        void LoadAllData(IExerciseProvider exerciseProvider,
                         IExerciseSetProvider exerciseSetProvider,
                         IQuestionProvider questionProvider);
    }
}
