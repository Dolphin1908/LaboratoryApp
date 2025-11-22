namespace LaboratoryApp.src.Data.Providers.Common
{
    public interface ISQLiteDataProvider : IDisposable
    {
        public string DatabaseName { get; }

        Task<int> ExecuteNonQueryAsync(string query, object? parameters = null);
        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object? parameters = null);
    }
}
