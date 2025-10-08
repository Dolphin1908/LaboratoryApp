using LaboratoryApp.src.Core.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Authorization
{
    public interface IExerciseSetAccessProvider
    {
        Task CreateNewAccess(ExerciseSetAccess access);
        Task<List<ExerciseSetAccess>> GetAllAccess();
        Task<List<ExerciseSetAccess>> GetAllAccessByUserId(long userId);
    }
}
