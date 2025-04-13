using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.Models.English;
using LaboratoryApp.Database.Provider;
using LaboratoryApp.Support.Helpers;

namespace LaboratoryApp.Services
{
    public class EnglishService
    {
        private readonly string _englishDbPath = AppConfigHelper.GetKey("EnglishDbPath");

        public List<WordModel> GetAllWords()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<WordModel>("SELECT * FROM Words");
        }

        public List<PosModel> GetAllPos()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<PosModel>("SELECT * FROM Pos");
        }

        public List<ExampleModel> GetAllExamples()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<ExampleModel>("SELECT * FROM Examples");
        }

        public List<DefinitionModel> GetAllDefinitions()
        {
            using var db = new SQLiteDataProvider(_englishDbPath);
            return db.ExecuteQuery<DefinitionModel>("SELECT * FROM Definitions");
        }
    }
}
