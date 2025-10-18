using LaboratoryApp.src.Core.Models.Authorization;
using LaboratoryApp.src.Data.Providers.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Caches.Authorization
{
    public interface IAuthorizationCache
    {
        List<ExerciseSetAccess> AllExerciseSetAccess { get; set; }

        void LoadAllData(IExerciseSetAccessProvider provider);
    }
}
