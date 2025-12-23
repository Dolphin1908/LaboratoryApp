using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.Domain.Interfaces.Services.Operations;
using LaboratoryApp.Domain.Models.Authorization;

namespace LaboratoryApp.src.Services.Operations
{
    public class ExerciseAccessService : IExerciseAccessService
    {
        private readonly IExerciseAccessProvider _exerciseAccessProvider;

        public ExerciseAccessService(IExerciseAccessProvider exerciseAccessProvider)
        {
            _exerciseAccessProvider = exerciseAccessProvider;
        }

        public async Task SaveNewExerciseAccessAsync(ExerciseAccess newExerciseAccess)
        {
            await _exerciseAccessProvider.CreateNewExerciseAccessAsync(newExerciseAccess);
        }
    }
}
