using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Authorization;
using LaboratoryApp.src.Data.Providers.Authorization;

namespace LaboratoryApp.src.Core.Caches.Authorization
{
    public class AuthorizationCache : IAuthorizationCache
    {
        private readonly object _lock = new();

        public List<ExerciseSetAccess> AllExerciseSetAccess { get; set; } = new();

        public void LoadAllData(IExerciseSetAccessProvider provider)
        {
            lock(_lock)
            {
                AllExerciseSetAccess = provider.GetAllAccess().GetAwaiter().GetResult();
            }
        }
    }
}
