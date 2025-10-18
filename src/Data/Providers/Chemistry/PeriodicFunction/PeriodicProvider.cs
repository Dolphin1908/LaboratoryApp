using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Data.Providers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Element> GetAllElements()
        {
            return _sqliteDb.ExecuteQuery<Element>($"SELECT * FROM {CollectionName.Elements}");
        }
    }
}
