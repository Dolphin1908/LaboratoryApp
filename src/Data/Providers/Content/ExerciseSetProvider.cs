using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Content
{
    public class ExerciseSetProvider : IExerciseSetProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<ExerciseSet> _exerciseSetCollection;

        public ExerciseSetProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);
            _exerciseSetCollection = _mongoDb.GetCollection<ExerciseSet>(CollectionName.ExerciseSets);
        }

        /// <summary>
        /// Lấy bộ bài tập theo Id
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public async Task<ExerciseSet?> GetExerciseSetByIdAsync(long setId)
        {
            var filter = Builders<ExerciseSet>.Filter.Eq(es => es.Id, setId);
            var result = await _exerciseSetCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Lấy tất cả bộ bài tập
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExerciseSet>> GetAllExerciseSetsAsync()
        {
            var sets = await _exerciseSetCollection.Find(FilterDefinition<ExerciseSet>.Empty).ToListAsync();
            return sets;
        }

        /// <summary>
        /// Tạo mới bộ bài tập
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public async Task CreateNewExerciseSetAsync(ExerciseSet set)
        {
            await _exerciseSetCollection.InsertOneAsync(set);
        }

        /// <summary>
        /// Cập nhật bộ bài tập
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public async Task UpdateExerciseSetAsync(ExerciseSet set)
        {
            await _exerciseSetCollection.ReplaceOneAsync(es => es.Id == set.Id, set);
        }

        /// <summary>
        /// Xóa bộ bài tập
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public async Task DeleteExerciseSetAsync(ExerciseSet set)
        {
            await _exerciseSetCollection.DeleteOneAsync(es => es.Id == set.Id);
        }
    }
}
