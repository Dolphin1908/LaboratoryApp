using LaboratoryApp.Domain.Models.Content;

namespace LaboratoryApp.src.Data.Providers.Assignment.ExerciseSetFunction
{
    public interface IExerciseSetProvider
    {
        Task<ExerciseSet?> GetExerciseSetByIdAsync(long setId);
        Task<List<ExerciseSet>> GetAllExerciseSetsAsync();
        Task CreateNewExerciseSetAsync(ExerciseSet set);
        Task UpdateExerciseSetAsync(ExerciseSet set);
        Task DeleteExerciseSetAsync(ExerciseSet set);
    }
}
