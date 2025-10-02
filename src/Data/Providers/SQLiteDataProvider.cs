using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SQLite;
using System.Configuration;
using System.Xml.Linq;
using LaboratoryApp.src.Data.Providers.Interfaces;


namespace LaboratoryApp.src.Data.Providers
{
    public class SQLiteDataProvider : ISQLiteDataProvider
    {
        private SQLiteConnection _connection;

        public string DatabaseName { get; }

        // Constructor
        public SQLiteDataProvider(string dbPath)
        {
            if(!File.Exists(dbPath))
            {
                throw new FileNotFoundException("Database file not found.", dbPath);
            }

            var connectionString = $"Data Source={dbPath};Version=3;";
            _connection = new SQLiteConnection(connectionString);
            DatabaseName = Path.GetFileNameWithoutExtension(dbPath);
            OpenConnection();
        }

        // Open connection method
        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        // Close connection method
        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        // ExecuteNonQuery method for INSERT, UPDATE, DELETE statement
        public int ExecuteNonQuery(string query, List<SQLiteParameter> parameters = null)
        {
            using (var command = new SQLiteCommand(query, _connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                return command.ExecuteNonQuery();
            }
        }

        // ExecuteQuery method for SELECT statement with return type is DataTable
        public List<T> ExecuteQuery<T>(string query, List<SQLiteParameter> parameters = null) where T : new()
        {
            using (var command = new SQLiteCommand(query, _connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                using (var adapter = new SQLiteDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    var result = new List<T>();

                    // Convert DataTable to List<T> (with T is a data type)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var item = new T();
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            if (dataTable.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                            {
                                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                var safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], propType);
                                prop.SetValue(item, safeValue);
                            }
                        }
                        result.Add(item);
                    }

                    return result;
                }
            }
        }

        // Dispose method
        public void Dispose()
        {
            CloseConnection();
            _connection.Dispose();
        }
    }
}
