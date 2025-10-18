using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Common
{
    public interface ISQLiteDataProvider : IDisposable
    {
        public string DatabaseName { get; }

        int ExecuteNonQuery(string query, List<SQLiteParameter> parameters = null);
        List<T> ExecuteQuery<T>(string query, List<SQLiteParameter> parameters = null) where T : new();
    }
}
