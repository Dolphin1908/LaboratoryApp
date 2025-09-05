using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Data.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.English
{
    public class EnglishService : IEnglishService
    {
        private readonly string _englishDbPath = ConfigurationManager.AppSettings["EnglishDbPath"];
        private readonly string _mongoDbPath = SecureConfigHelper.Decrypt(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);

        #region DiaryMongoDB
        public void AddDiary(DiaryContent diary)
        {
            using var db = new MongoDBProvider(_mongoDbPath, "english");
            db.Insert("diaries", diary);
        }
        public List<DiaryContent> GetAllDiaries()
        {
            using var db = new MongoDBProvider(_mongoDbPath, "english");
            return db.GetAll<DiaryContent>("diaries");
        }
        public void UpdateDiary(DiaryContent diary)
        {
            using var db = new MongoDBProvider(_mongoDbPath, "english");
            db.Update("diaries", diary.Id, diary);
        }
        public void DeleteDiary(long id)
        {
            using var db = new MongoDBProvider(_mongoDbPath, "english");
            db.Delete<DiaryContent>("diaries", id);
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
