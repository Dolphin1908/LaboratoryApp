using LaboratoryApp.Domain.Models.English.DictionaryFunction;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;

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
        public async Task<List<Word>> GetAllWordsAsync()
        {
            var query = $"SELECT * FROM {CollectionName.Words}";

            var words = await _sqliteDb.ExecuteQueryAsync<Word>(query);

            return words.ToList();
        }

        /// <summary>
        /// Get all parts of speech from the SQLite database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Pos>> GetAllPosAsync()
        {
            var query = $"SELECT * FROM {CollectionName.Pos}";

            var posList = await _sqliteDb.ExecuteQueryAsync<Pos>(query);

            return posList.ToList();
        }

        /// <summary>
        /// Get all examples from the SQLite database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Example>> GetAllExamplesAsync()
        {
            var query = $"SELECT * FROM {CollectionName.Examples}";

            var examples = await _sqliteDb.ExecuteQueryAsync<Example>(query);

            return examples.ToList();
        }

        /// <summary>
        /// Get all definitions from the SQLite database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Definition>> GetAllDefinitionsAsync()
        {
            var query = $"SELECT * FROM {CollectionName.Definitions}";

            var definitions = await _sqliteDb.ExecuteQueryAsync<Definition>(query);

            return definitions.ToList();
        }
    }
}
