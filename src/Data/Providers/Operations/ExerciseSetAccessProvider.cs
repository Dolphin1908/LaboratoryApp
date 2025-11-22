using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.Domain.Models.Authorization;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Operations
{
    public class ExerciseSetAccessProvider : IExerciseSetAccessProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public ExerciseSetAccessProvider(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthorizationMongoDB);

        public Task CreateNewAccess(ExerciseSetAccess access)
        {
            _mongoDb.Insert(CollectionName.ExerciseSetAccess, access);
            return Task.CompletedTask;
        }

        public Task<List<ExerciseSetAccess>> GetAllAccess()
        {
            var result = _mongoDb.GetAll<ExerciseSetAccess>(CollectionName.ExerciseSetAccess);
            return Task.FromResult(result);
        }

        public Task<List<ExerciseSetAccess>> GetAllAccessByUserId(long userId)
        {
            var filter = Builders<ExerciseSetAccess>.Filter.Eq(esa => esa.UserId, userId);
            var result = _mongoDb.GetAll(CollectionName.ExerciseSetAccess, filter);
            return Task.FromResult(result);
        }
    }
}
