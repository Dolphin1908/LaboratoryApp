using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using LaboratoryApp.src.Core.Helpers;

using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Core.Models.English.ExerciseFunction;

using LaboratoryApp.src.Data.Providers;

namespace LaboratoryApp.src.Services.English
{
    public class EnglishService : IEnglishService
    {
        private readonly string _englishDbPath = ConfigurationManager.AppSettings["EnglishDbPath"];
        private readonly string _mongoDbPath = SecureConfigHelper.Decrypt(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);

        #region ExerciseMongoDB
        public void AddExerciseSet(ExerciseSet exerciseSet)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "english");
                db.Insert("exercises", exerciseSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding an exercise set: {ex.Message}");
                return;
            }
        }
        #endregion

        #region DiaryMongoDB
        /// <summary>
        /// Add a new diary entry to the MongoDB database.
        /// </summary>
        /// <param name="diary"></param>
        public void AddDiary(DiaryContent diary)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "english");
                db.Insert("diaries", diary);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding a diary entry: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Get all diary entries from the MongoDB database.
        /// </summary>
        /// <returns></returns>
        public List<DiaryContent> GetAllDiaries()
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "english");
                return db.GetAll<DiaryContent>("diaries");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching diary entries: {ex.Message}");
                return new List<DiaryContent>();
            }
        }

        /// <summary>
        /// Update an existing diary entry.
        /// </summary>
        /// <param name="diary"></param>
        public void UpdateDiary(DiaryContent diary)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "english");
                db.Update("diaries", diary.Id, diary);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the diary entry: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Delete a diary entry by its ID.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDiary(long id)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "english");
                db.Delete<DiaryContent>("diaries", id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the diary entry: {ex.Message}");
                return;
            }
        }
        #endregion

        #region DictionarySQLite
        public List<Word> GetAllWords()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<Word>("SELECT * FROM Words");
        }

        public List<Pos> GetAllPos()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<Pos>("SELECT * FROM Pos");
        }

        public List<Example> GetAllExamples()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<Example>("SELECT * FROM Examples");
        }

        public List<Definition> GetAllDefinitions()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<Definition>("SELECT * FROM Definitions");
        }
        #endregion
    }
}
