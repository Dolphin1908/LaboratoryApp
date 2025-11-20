using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Common
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

        /// <summary>
        /// Lấy 1 "cổng giao tiếp" để làm việc với Database MongoDB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));
            return _database.GetCollection<T>(collectionName);
        }

        public List<T> GetAll<T>(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));

            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(FilterDefinition<T>.Empty).ToList();
        }

        public List<T> GetAll<T>(string collectionName, FilterDefinition<T> filter)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));

            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(filter).ToList();
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

        public void DeleteAll<T>(string collectionName, string field, long id)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name must be provided.", nameof(collectionName));

            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(field, id);
            collection.DeleteMany(filter);
        }

        public void Dispose()
        {
            // Dispose of the MongoClient if necessary
            // In this case, MongoClient is thread-safe and can be reused, so we don't dispose it here.
            // If you want to dispose it, you can do so here.
        }
    }
}
