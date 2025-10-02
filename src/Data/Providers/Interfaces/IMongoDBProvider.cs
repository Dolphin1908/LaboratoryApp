using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Interfaces
{
    public interface IMongoDBProvider : IDisposable
    {
        public string DatabaseName { get; }

        List<T> GetAll<T>(string collectionName);
        T? GetOne<T>(string collectionName, FilterDefinition<T> filter);
        void Insert<T>(string collectionName, T document);
        void Update<T>(string collectionName, long id, T document);
        void Delete<T>(string collectionName, long id);
    }
}
