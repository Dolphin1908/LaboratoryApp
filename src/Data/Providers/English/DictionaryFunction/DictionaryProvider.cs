using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Data.Providers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.English.DictionaryFunction
{
    public class DictionaryProvider : IDictionaryProvider
    {
        private readonly ISQLiteDataProvider _sqliteDb;

        public DictionaryProvider(IEnumerable<ISQLiteDataProvider> sqliteDb)
        {
            _sqliteDb = sqliteDb.First(d => d.DatabaseName == DatabaseName.EnglishSQLite);
        }

        /// <summary>
        /// Get all words from the SQLite database.
        /// </summary>
        /// <returns></returns>
        public List<Word> GetAllWords()
        {
            return _sqliteDb.ExecuteQuery<Word>($"SELECT * FROM {CollectionName.Words}");
        }

        /// <summary>
        /// Get all parts of speech from the SQLite database.
        /// </summary>
        /// <returns></returns>
        public List<Pos> GetAllPos()
        {
            return _sqliteDb.ExecuteQuery<Pos>($"SELECT * FROM {CollectionName.Pos}");
        }

        /// <summary>
        /// Get all examples from the SQLite database.
        /// </summary>
        /// <returns></returns>
        public List<Example> GetAllExamples()
        {
            return _sqliteDb.ExecuteQuery<Example>($"SELECT * FROM {CollectionName.Examples}");
        }

        /// <summary>
        /// Get all definitions from the SQLite database.
        /// </summary>
        /// <returns></returns>
        public List<Definition> GetAllDefinitions()
        {
            return _sqliteDb.ExecuteQuery<Definition>($"SELECT * FROM {CollectionName.Definitions}");
        }
    }
}
