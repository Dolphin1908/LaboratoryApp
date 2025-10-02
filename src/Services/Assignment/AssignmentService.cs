using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Assignment;

using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Data.Providers.Interfaces;

namespace LaboratoryApp.src.Services.Assignment
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IMongoDBProvider _mongoDb;

        public AssignmentService(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);

        #region ExerciseSet
        /// <summary>
        /// Handle adding a new exercise set to the database
        /// </summary>
        /// <param name="set"></param>
        public void AddExerciseSet(ExerciseSet set)
        {
            try
            {
                _mongoDb.Insert(CollectionName.ExerciseSets, set);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding a exercise set: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Handle updating an existing exercise set in the database
        /// </summary>
        /// <param name="set"></param>
        public void UpdateExerciseSet(ExerciseSet set)
        {
            try
            {
                _mongoDb.Update(CollectionName.ExerciseSets, set.Id, set);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the exercise set: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Handle retrieving all exercise sets from the database
        /// </summary>
        public List<ExerciseSet> GetAllExerciseSets()
        {
            try
            {
                return _mongoDb.GetAll<ExerciseSet>(CollectionName.ExerciseSets);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving exercise sets: {ex.Message}");
                return new List<ExerciseSet>();
            }
        }

        /// <summary>
        /// Handle deleting an exercise set from the database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteExerciseSet(long id)
        {
            try
            {
                _mongoDb.Delete<ExerciseSet>(CollectionName.ExerciseSets, id); // TODO: Replace 0 with actual id
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the exercise set: {ex.Message}");
                return;
            }
        }
        #endregion

        #region Exercise

        #endregion
    }
}
