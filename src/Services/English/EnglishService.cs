using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Data.Providers;

namespace LaboratoryApp.src.Services.English
{
    public class EnglishService
    {
        private readonly string _englishDbPath = ConfigurationManager.AppSettings["EnglishDbPath"];

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
    }
}
