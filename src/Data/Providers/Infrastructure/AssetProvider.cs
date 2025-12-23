using LaboratoryApp.Domain.Enums.Core;
using LaboratoryApp.Domain.Interfaces.Providers.Infrastructure;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Models.Core;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LaboratoryApp.src.Data.Providers.Infrastructure
{
    public class AssetProvider : IAssetProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly ICounterService _counterService;
        private readonly IMongoCollection<Asset> _assetCollection;

        public AssetProvider(IEnumerable<IMongoDBProvider> mongoDb,
                             ICounterService counterService)
        {
            _mongoDb = mongoDb.First(db => db.DatabaseName == DatabaseName.InfrastructureMongoDB);
            _counterService = counterService;
            _assetCollection = _mongoDb.GetCollection<Asset>(CollectionName.Assets);
        }

        public async Task<Asset?> GetByIdAsync(long id)
        {
            return await _assetCollection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Asset?> GetByHashAsync(string hashValue, AssetType assetType)
        {
            return await _assetCollection.Find(a => a.HashValue == hashValue && a.Type == assetType).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Lấy dữ liệu theo trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ownerId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<Asset>> GetPagedAsync(int pageIndex, int pageSize, long? ownerId, AssetType? type)
        {
            var query = _assetCollection.AsQueryable();

            if (ownerId.HasValue)
            {
                query = query.Where(a => a.OwnerUserId == ownerId);
            }

            if (type.HasValue)
            {
                query = query.Where(a => a.Type == type);
            }

            var projectedQuery = query.Select(a => new Asset
            {
                Id = a.Id,
                FileName = a.FileName,
                Type = a.Type,
                HashValue = a.HashValue,
                ReferenceCount = a.ReferenceCount,
                CreatedAt = a.CreatedAt,
            });

            return await projectedQuery.OrderByDescending(a => a.CreatedAt)
                                       .Skip((pageIndex - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
        }

        public async Task AddAssetAsync(Asset asset)
        {
            asset.Id = _counterService.GetNextId(CollectionName.Assets);
            await _assetCollection.InsertOneAsync(asset);
        }

        public async Task UpdateAssetAsync(Asset asset)
        {
            await _assetCollection.ReplaceOneAsync(a => a.Id == asset.Id, asset);
        }

        public async Task DeleteAssetAsync(Asset asset)
        {
            await _assetCollection.DeleteOneAsync(a => a.Id == asset.Id);
        }
    }
}
