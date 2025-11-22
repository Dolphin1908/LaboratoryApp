using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Models.Content;

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
