using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Content
{
    public class ExerciseProvider : IExerciseProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<Exercise> _exerciseCollection;

        public ExerciseProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);
            _exerciseCollection = _mongoDb.GetCollection<Exercise>(CollectionName.Exercises);
        }

        /// <summary>
        /// Lấy tất cả bài tập
        /// </summary>
        /// <returns></returns>
        public async Task<List<Exercise>> GetAllExercisesAsync()
        {
            var exercises = await _exerciseCollection.Find(FilterDefinition<Exercise>.Empty).ToListAsync();
            return exercises;
        }

        /// <summary>
        /// Lấy bài tập theo danh sách Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<Exercise>> GetExercisesByIdsAsync(List<long> ids)
        {
            var filter = Builders<Exercise>.Filter.In(e => e.Id, ids);
            var exercises = await _exerciseCollection.Find(filter).ToListAsync();
            return exercises;
        }

        /// <summary>
        /// Lấy bài tập theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Exercise?> GetExerciseByIdAsync(long id)
        {
            var exercise = await _exerciseCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
            return exercise;
        }

        /// <summary>
        /// Tạo mới bài tập
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns></returns>
        public async Task CreateNewExerciseAsync(Exercise exercise)
        {
            await _exerciseCollection.InsertOneAsync(exercise);
        }

        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            var filter = Builders<Exercise>.Filter.Eq(e => e.Id, exercise.Id);
            await _exerciseCollection.ReplaceOneAsync(filter, exercise);
        }

        public async Task DeleteExerciseAsync(long id)
        {
            var filter = Builders<Exercise>.Filter.Eq(e => e.Id, id);
            await _exerciseCollection.DeleteOneAsync(filter);
        }
    }
}
