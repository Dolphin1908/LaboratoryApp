using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Assignment.ExerciseFunction
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
        /// Tạo mới bài tập
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns></returns>
        public async Task CreateNewExerciseAsync(Exercise exercise)
        {
            await _exerciseCollection.InsertOneAsync(exercise);
        }
    }
}
