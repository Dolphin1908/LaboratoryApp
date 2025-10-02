using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using LaboratoryApp.src.Constants;

using LaboratoryApp.src.Core.Helpers;

using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;

using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Data.Providers.Interfaces;

namespace LaboratoryApp.src.Services.English
{
    public class EnglishService : IEnglishService
    {
        private readonly ISQLiteDataProvider _sqliteDb;
        private readonly IMongoDBProvider _mongoDb;

        public EnglishService(IEnumerable<ISQLiteDataProvider> sqliteDb,
                              IEnumerable<IMongoDBProvider> mongoDb)
        {
            _sqliteDb = sqliteDb.First(d => d.DatabaseName == DatabaseName.EnglishSQLite);
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.EnglishMongoDB);
        }

        #region DiaryMongoDB
        /// <summary>
        /// Add a new diary entry to the MongoDB database.
        /// </summary>
        /// <param name="diary"></param>
        public void AddDiary(DiaryContent diary)
        {
            try
            {
                _mongoDb.Insert(CollectionName.Diaries, diary);
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
                return _mongoDb.GetAll<DiaryContent>(CollectionName.Diaries);
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
                _mongoDb.Update(CollectionName.Diaries, diary.Id, diary);
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
                _mongoDb.Delete<DiaryContent>(CollectionName.Diaries, id);
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
            try
            {
                return _sqliteDb.ExecuteQuery<Word>($"SELECT * FROM {CollectionName.Words}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching words: {ex.Message}");
                return new List<Word>();
            }
        }

        public List<Pos> GetAllPos()
        {
            try
            {
                return _sqliteDb.ExecuteQuery<Pos>($"SELECT * FROM {CollectionName.Pos}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching pos: {ex.Message}");
                return new List<Pos>();
            }
        }

        public List<Example> GetAllExamples()
        {
            try
            {
                return _sqliteDb.ExecuteQuery<Example>($"SELECT * FROM {CollectionName.Examples}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching examples: {ex.Message}");
                return new List<Example>();
            }
        }

        public List<Definition> GetAllDefinitions()
        {
            try
            {
                return _sqliteDb.ExecuteQuery<Definition>($"SELECT * FROM {CollectionName.Definitions}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching definitions: {ex.Message}");
                return new List<Definition>();
            }
        }
        #endregion
    }
}
