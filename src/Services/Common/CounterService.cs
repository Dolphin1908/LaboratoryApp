using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Models.Core;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Services.Common
{
    public class CounterService : ICounterService
    {
        private IMongoDBProvider _mongoDb;

        public CounterService(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.HelperMongoDB);
        }

        public long GetNextId(string collectionName)
        {
            var data = _mongoDb.GetOne<CounterModel>(CollectionName.Counters, Builders<CounterModel>.Filter.Eq(c => c.CollectionName, collectionName));

            if (data == null)
            {
                data = new CounterModel
                {
                    Id = _mongoDb.GetAll<CounterModel>(CollectionName.Counters).Count + 1,
                    CollectionName = collectionName,
                    Seq = 0
                };

                _mongoDb.Insert(CollectionName.Counters, data);
            }

            data.Seq++;
            _mongoDb.Update(CollectionName.Counters, data.Id, data);

            return data.Seq;
        }
    }
}
