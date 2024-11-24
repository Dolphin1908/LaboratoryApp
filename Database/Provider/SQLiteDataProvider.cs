using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Data;
using System.Configuration;

namespace LaboratoryApp.Database.Provider
{
    public class SQLiteDataProvider
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;
        private static SQLiteDataProvider _instance;
        public static SQLiteDataProvider Instance
        {
            get
            {
                if(_instance == null )
                    _instance = new SQLiteDataProvider();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public SQLiteDataProvider()
        {
            var dbPath = ConfigurationManager.AppSettings["SQLitePath"];
            _connectionString = $"Data Source={dbPath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
            OpenConnection();
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

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

        public DataTable ExecuteQuery(string query, List<SQLiteParameter> parameters = null)
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
                    return dataTable;
                }
            }
        }

        public void Dispose()
        {
            CloseConnection();
            _connection.Dispose();
            _instance = null;
        }
    }
}
