using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Models.Assignment;

using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Assignment
{
    public class AssignmentProvider : IAssignmentProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public AssignmentProvider(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);

        #region ExerciseSet
        /// <summary>
        /// Lấy bộ bài tập theo Id
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public Task<ExerciseSet?> GetExerciseSetById (long setId)
        {
            var filter = Builders<ExerciseSet>.Filter.Eq(es => es.Id, setId);
            var result = _mongoDb.GetOne(CollectionName.ExerciseSets, filter);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Lấy tất cả bộ bài tập
        /// </summary>
        /// <returns></returns>
        public Task<List<ExerciseSet>> GetAllExerciseSets()
        {
            var sets = _mongoDb.GetAll<ExerciseSet>(CollectionName.ExerciseSets);
            return Task.FromResult(sets);
        }

        /// <summary>
        /// Tạo mới bộ bài tập
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Task CreateNewExerciseSet(ExerciseSet set)
        {
            _mongoDb.Insert(CollectionName.ExerciseSets, set);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Cập nhật bộ bài tập
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Task UpdateExerciseSet (ExerciseSet set)
        {
            _mongoDb.Update(CollectionName.ExerciseSets, set.Id, set);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Xóa bộ bài tập
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Task DeleteExerciseSet(ExerciseSet set)
        {
            _mongoDb.Delete<ExerciseSet>(CollectionName.ExerciseSets, set.Id);
            return Task.CompletedTask;
        }
        #endregion

        #region Exercise
        /// <summary>
        /// Lấy tất cả bài tập
        /// </summary>
        /// <returns></returns>
        public Task<List<Exercise>> GetAllExercises()
        {
            var exercises = _mongoDb.GetAll<Exercise>(CollectionName.Exercises);
            return Task.FromResult(exercises);
        }

        /// <summary>
        /// Tạo mới bài tập
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns></returns>
        public Task CreateNewExercise(Exercise exercise)
        {
            _mongoDb.Insert(CollectionName.Exercises, exercise);
            return Task.CompletedTask;
        }
        #endregion
    }
}
