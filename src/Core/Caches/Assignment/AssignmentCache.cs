using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Models.Content;

namespace LaboratoryApp.src.Core.Caches.Assignment
{
    public class AssignmentCache : IAssignmentCache
    {
        private readonly object _lock = new();

        public List<ExerciseSet> AllExerciseSets { get; set; } = new();
        public List<Exercise> AllExercises { get; set; } = new();

        public void LoadAllData(IExerciseProvider exerciseProvider,
                                IExerciseSetProvider exerciseSetProvider,
                                IQuestionProvider questionProvider)
        {
            lock (_lock)
            {
                LoadAllDataAsync(exerciseProvider, exerciseSetProvider, questionProvider);
            }
        }

        private async void LoadAllDataAsync(IExerciseProvider exerciseProvider,
                                            IExerciseSetProvider exerciseSetProvider,
                                            IQuestionProvider questionProvider)
        {
            AllExercises = await exerciseProvider.GetAllExercisesAsync();
            AllExerciseSets = await exerciseSetProvider.GetAllExerciseSetsAsync();
        }
    }
}