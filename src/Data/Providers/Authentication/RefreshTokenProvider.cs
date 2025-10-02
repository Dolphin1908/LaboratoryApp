using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Interfaces;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Constants;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public RefreshTokenProvider(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);

        public Task CreateAsync(RefreshToken token)
        {
            _mongoDb.Insert(CollectionName.RefreshTokens, token);
            return Task.CompletedTask;
        }

        public Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(t => t.Token, token);
            var rt = _mongoDb.GetOne(CollectionName.RefreshTokens, filter);
            return Task.FromResult(rt);
        }

        public Task UpdateAsync(RefreshToken token)
        {
            _mongoDb.Update(CollectionName.RefreshTokens, token.Id, token);
            return Task.CompletedTask;
        }

        public Task<RefreshToken?> GetLatestByUserIdAsync(long userId)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(t => t.UserId, userId);
            var sort = Builders<RefreshToken>.Sort.Descending(t => t.CreatedAt);
            return Task.FromResult(
                _mongoDb.GetAll<RefreshToken>(CollectionName.RefreshTokens)
                   .Where(t => t.UserId == userId)
                   .OrderByDescending(t => t.CreatedAt)
                   .FirstOrDefault()
            );
        }

        public long GetNextId()
        {
            var tokens = _mongoDb.GetAll<RefreshToken>(CollectionName.RefreshTokens);
            return tokens.Count == 0 ? 1 : tokens.Max(t => t.Id) + 1;
        }

    }
}
