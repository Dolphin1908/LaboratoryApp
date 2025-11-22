using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.Domain.Models.Authorization;

namespace LaboratoryApp.src.Core.Caches.Authorization
{
    public class AuthorizationCache : IAuthorizationCache
    {
        private readonly object _lock = new();

        public List<ExerciseSetAccess> AllExerciseSetAccess { get; set; } = new();

        public void LoadAllData(IExerciseSetAccessProvider provider)
        {
            lock (_lock)
            {
                AllExerciseSetAccess = provider.GetAllAccess().GetAwaiter().GetResult();
            }
        }
    }
}
