using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MongoDB.Bson;
using MongoDB.Driver;

using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Helper;
using LaboratoryApp.src.Data.Providers.Interfaces;

namespace LaboratoryApp.src.Services.Counter
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
