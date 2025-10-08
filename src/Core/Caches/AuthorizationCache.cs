using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Authorization;
using LaboratoryApp.src.Data.Providers.Authorization;

namespace LaboratoryApp.src.Core.Caches
{
    public static class AuthorizationCache
    {
        private static readonly object _lock = new();

        public static List<ExerciseSetAccess> AllExerciseSetAccess { get; set; } = new();

        public static void LoadAllData(IExerciseSetAccessProvider provider)
        {
            lock(_lock)
            {
                AllExerciseSetAccess = provider.GetAllAccess().GetAwaiter().GetResult();
            }
        }
    }
}
