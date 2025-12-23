using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.Domain.Models.Authorization;

namespace LaboratoryApp.src.Core.Caches.Authorization
{
    public class AuthorizationCache : IAuthorizationCache
    {
        private readonly object _lock = new();

        public List<ExerciseAccess> AllExerciseAccess { get; set; } = new();

        public void LoadAllData(IExerciseAccessProvider provider)
        {
            lock (_lock)
            {
                LoadAllDataAsync(provider);
            }
        }

        private async void LoadAllDataAsync(IExerciseAccessProvider provider)
        {
            AllExerciseAccess = await provider.GetAllExerciseAccessAsync();
        }
    }
}
