using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Common
{
    public interface IMongoDBProvider : IDisposable
    {
        string DatabaseName { get; }
        IMongoCollection<T> GetCollection<T>(string collectionName);

        List<T> GetAll<T>(string collectionName);
        List<T> GetAll<T>(string collectionName, FilterDefinition<T> filter);
        T? GetOne<T>(string collectionName, FilterDefinition<T> filter);
        void Insert<T>(string collectionName, T document);
        void Update<T>(string collectionName, long id, T document);
        void Delete<T>(string collectionName, long id);
    }
}
