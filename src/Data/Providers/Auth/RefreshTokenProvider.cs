using LaboratoryApp.Domain.Interfaces.Providers.Auth;
using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Auth
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<RefreshToken> _refreshTokenCollection;

        public RefreshTokenProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);
            _refreshTokenCollection = _mongoDb.GetCollection<RefreshToken>(CollectionName.RefreshTokens);
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            await _refreshTokenCollection.InsertOneAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(t => t.Token, token);
            return await _refreshTokenCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(t => t.Id, refreshToken.Id);
            await _refreshTokenCollection.ReplaceOneAsync(filter, refreshToken);
        }

        public async Task<RefreshToken?> GetLatestByUserIdAsync(long userId)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(t => t.UserId, userId);
            var sort = Builders<RefreshToken>.Sort.Descending(t => t.CreatedAt);
            return await _refreshTokenCollection.Find(filter).Sort(sort).FirstOrDefaultAsync();
        }

        //public Task CreateAsync(RefreshToken token)
        //{
        //    _mongoDb.Insert(CollectionName.RefreshTokens, token);
        //    return Task.CompletedTask;
        //}

        //public Task<RefreshToken?> GetByTokenAsync(string token)
        //{
        //    var filter = Builders<RefreshToken>.Filter.Eq(t => t.Token, token);
        //    var rt = _mongoDb.GetOne(CollectionName.RefreshTokens, filter);
        //    return Task.FromResult(rt);
        //}

        //public Task UpdateAsync(RefreshToken token)
        //{
        //    _mongoDb.Update(CollectionName.RefreshTokens, token.Id, token);
        //    return Task.CompletedTask;
        //}

        //public Task<RefreshToken?> GetLatestByUserIdAsync(long userId)
        //{
        //    var filter = Builders<RefreshToken>.Filter.Eq(t => t.UserId, userId);
        //    var sort = Builders<RefreshToken>.Sort.Descending(t => t.CreatedAt);
        //    return Task.FromResult(
        //        _mongoDb.GetAll<RefreshToken>(CollectionName.RefreshTokens)
        //           .Where(t => t.UserId == userId)
        //           .OrderByDescending(t => t.CreatedAt)
        //           .FirstOrDefault()
        //    );
        //}

        //public long GetNextId()
        //{
        //    var tokens = _mongoDb.GetAll<RefreshToken>(CollectionName.RefreshTokens);
        //    return tokens.Count == 0 ? 1 : tokens.Max(t => t.Id) + 1;
        //}
    }
}
