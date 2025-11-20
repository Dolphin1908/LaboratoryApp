using LaboratoryApp.Domain.Models.Authorization;
using LaboratoryApp.src.Data.Providers.Authorization;

namespace LaboratoryApp.src.Core.Caches.Authorization
{
    public interface IAuthorizationCache
    {
        List<ExerciseSetAccess> AllExerciseSetAccess { get; set; }

        void LoadAllData(IExerciseSetAccessProvider provider);
    }
}
