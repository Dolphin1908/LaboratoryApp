using LaboratoryApp.Domain.Interfaces.Providers.Operations;
using LaboratoryApp.Domain.Models.Authorization;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Operations
{
    public class ExerciseAccessProvider : IExerciseAccessProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<ExerciseAccess> _exerciseAccessCollection;

        public ExerciseAccessProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthorizationMongoDB);
            _exerciseAccessCollection = _mongoDb.GetCollection<ExerciseAccess>(CollectionName.ExerciseAccess);
        }

        public async Task<List<ExerciseAccess>> GetAllExerciseAccessAsync()
        {
            var exerciseAccessList = await _exerciseAccessCollection.Find(FilterDefinition<ExerciseAccess>.Empty).ToListAsync();
            return exerciseAccessList;
        }

        public async Task<List<ExerciseAccess>> GetAllExerciseAccessByUserIdAsync(long userId)
        {
            var filter = Builders<ExerciseAccess>.Filter.Eq(esa => esa.UserId, userId);
            var exerciseAccessList = await _exerciseAccessCollection.Find(filter).ToListAsync();
            return exerciseAccessList;
        }

        public async Task CreateNewExerciseAccessAsync(ExerciseAccess access)
        {
            await _exerciseAccessCollection.InsertOneAsync(access);
        }
    }
}
