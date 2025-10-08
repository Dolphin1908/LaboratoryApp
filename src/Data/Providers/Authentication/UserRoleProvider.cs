using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Data.Providers.Interfaces;
using LaboratoryApp.src.Constants;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class UserRoleProvider : IUserRoleProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public UserRoleProvider(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);

        public async Task<UserRole?> GetUserRolesAsync (long userId)
        {
            var filter = Builders<UserRole>.Filter.Eq(ur => ur.UserId, userId);
            var userRole = _mongoDb.GetOne(CollectionName.UserRole, filter);
            return await Task.FromResult(userRole);
        }

        public Task CreateUserRole (UserRole userRole)
        {
            _mongoDb.Insert(CollectionName.UserRole, userRole);
            return Task.CompletedTask;
        }

        public long GetNextUserRoleId()
        {
            var userRoles = _mongoDb.GetAll<UserRole>(CollectionName.UserRole);
            return userRoles.Count > 0 ? userRoles.Max(ur => ur.Id) + 1 : 1;
        }
    }
}
