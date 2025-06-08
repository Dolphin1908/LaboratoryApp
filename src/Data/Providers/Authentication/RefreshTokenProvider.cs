using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Interfaces;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IMongoDBProvider _db;

        public RefreshTokenProvider(IMongoDBProvider db) => _db = db;

        public Task CreateAsync(RefreshToken token)
        {
            _db.Insert("refreshTokens", token);
            return Task.CompletedTask;
        }

        public Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(t => t.Token, token);
            var rt = _db.GetOne("refreshTokens", filter);
            return Task.FromResult(rt);
        }

        public Task UpdateAsync(RefreshToken token)
        {
            _db.Update("refreshTokens", token.Id, token);
            return Task.CompletedTask;
        }

        public Task<RefreshToken?> GetLatestByUserIdAsync(long userId)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(t => t.User.Id, userId);
            var sort = Builders<RefreshToken>.Sort.Descending(t => t.CreatedAt);
            return Task.FromResult(
                _db.GetAll<RefreshToken>("refreshTokens")
                   .Where(t => t.User.Id == userId)
                   .OrderByDescending(t => t.CreatedAt)
                   .FirstOrDefault()
            );
        }

        public long GetNextId()
        {
            var tokens = _db.GetAll<RefreshToken>("refreshTokens");
            return tokens.Count == 0 ? 1 : tokens.Max(t => t.Id) + 1;
        }

    }
}
