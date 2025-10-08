using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Models.Assignment;

using LaboratoryApp.src.Data.Providers.Interfaces;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Assignment
{
    public class AssignmentProvider : IAssignmentProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public AssignmentProvider(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);

        #region ExerciseSet
        public Task<ExerciseSet?> GetExerciseSetById (long setId)
        {
            var filter = Builders<ExerciseSet>.Filter.Eq(es => es.Id, setId);
            var result = _mongoDb.GetOne(CollectionName.ExerciseSets, filter);
            return Task.FromResult(result);
        }

        public Task<List<ExerciseSet>> GetAllExerciseSets()
        {
            var sets = _mongoDb.GetAll<ExerciseSet>(CollectionName.ExerciseSets);
            return Task.FromResult(sets);
        }

        public Task CreateNewExerciseSet(ExerciseSet set)
        {
            _mongoDb.Insert(CollectionName.ExerciseSets, set);
            return Task.CompletedTask;
        }

        public Task UpdateExerciseSet (ExerciseSet set)
        {
            _mongoDb.Update(CollectionName.ExerciseSets, set.Id, set);
            return Task.CompletedTask;
        }

        public Task DeleteExerciseSet(ExerciseSet set)
        {
            _mongoDb.Delete<ExerciseSet>(CollectionName.ExerciseSets, set.Id);
            return Task.CompletedTask;
        }
        
        #endregion

        public List<Exercise> GetAllExercisesBySetId(long setId)
        {
            var filter = Builders<ExerciseSet>.Filter.Eq(es => es.Id, setId);
            var set = _mongoDb.GetOne(CollectionName.ExerciseSets, filter);
            if (set == null) return new List<Exercise>();

            var exercises = new List<Exercise>();
            foreach (var exerciseId in set.ExerciseIds)
            {
                var exerciseFilter = Builders<Exercise>.Filter.Eq(e => e.Id, exerciseId);
                var exercise = _mongoDb.GetOne(CollectionName.Exercises, exerciseFilter);
                if (exercise != null)
                {
                    exercises.Add(exercise);
                }
            }
            return exercises;
        }
    }
}
