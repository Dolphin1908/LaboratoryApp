using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.Domain.Models.Authorization;

namespace LaboratoryApp.src.Core.Caches.Authorization
{
    public interface IAuthorizationCache
    {
        List<ExerciseSetAccess> AllExerciseSetAccess { get; set; }

        void LoadAllData(IExerciseSetAccessProvider provider);
    }
}
