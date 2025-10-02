using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Data.Providers.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers
{
    public class MongoDBProvider : IMongoDBProvider
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database; // Remove readonly

        public string DatabaseName { get; }

        public MongoDBProvider(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("MongoDB connection string must be provided.", nameof(connectionString));

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            DatabaseName = databaseName;
        }

        public List<T> GetAll<T>(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));

            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(FilterDefinition<T>.Empty).ToList();
        }

        public T? GetOne<T>(string collectionName, FilterDefinition<T> filter)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));

            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(filter).FirstOrDefault();
        }

        public void Insert<T>(string collectionName, T document)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));
            if (document == null)
                throw new ArgumentNullException(nameof(document), "Document to insert cannot be null.");

            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertOne(document);
        }

        public void Update<T>(string collectionName, long id, T document)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));
            if (document == null)
                throw new ArgumentNullException(nameof(document), "Document to update cannot be null.");

            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.ReplaceOne(filter, document);
        }

        public void Delete<T>(string collectionName, long id)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));

            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }

        public void Dispose()
        {
            // Dispose of the MongoClient if necessary
            // In this case, MongoClient is thread-safe and can be reused, so we don't dispose it here.
            // If you want to dispose it, you can do so here.
        }
    }
}
