using LaboratoryApp.Domain.Models.Chemistry.Common;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;

namespace LaboratoryApp.src.Data.Providers.Chemistry.PeriodicFunction
{
    public class PeriodicProvider : IPeriodicProvider
    {
        private readonly ISQLiteDataProvider _sqliteDb;

        public PeriodicProvider(IEnumerable<ISQLiteDataProvider> sqliteDb)
        {
            _sqliteDb = sqliteDb.First(d => d.DatabaseName == DatabaseName.ChemistrySQLite);
        }

        /// <summary>
        /// Get all elements from the SQLite database.
        /// </summary>
        /// <returns>All elements</returns>
        public async Task<List<Element>> GetAllElementsAsync()
        {
            var query = $"SELECT * FROM {CollectionName.Elements}";

            var elements = await _sqliteDb.ExecuteQueryAsync<Element>(query);

            return elements.ToList();
        }
    }
}
