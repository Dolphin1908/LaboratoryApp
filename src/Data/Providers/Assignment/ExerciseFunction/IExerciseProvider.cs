using LaboratoryApp.Domain.Models.Content;

namespace LaboratoryApp.src.Data.Providers.Assignment.ExerciseFunction
{
    public interface IExerciseProvider
    {
        Task<List<Exercise>> GetAllExercisesAsync();
        Task CreateNewExerciseAsync(Exercise exercise);
    }
}
