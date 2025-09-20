using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Data.Providers.Interfaces;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    internal class UserRoleProvider : IUserRoleProvider
    {
        private readonly IMongoDBProvider _db;

        public UserRoleProvider(IMongoDBProvider db) => _db = db;

        public async Task<UserRole?> GetUserRolesAsync (long userId)
        {
            var filter = Builders<UserRole>.Filter.Eq(ur => ur.UserId, userId);
            var userRole = _db.GetOne("userRoles", filter);
            return await Task.FromResult(userRole);
        }

        public long GetNextUserRoleId()
        {
            var userRoles = _db.GetAll<UserRole>("userRoles");
            return userRoles.Count > 0 ? userRoles.Max(ur => ur.Id) + 1 : 1;
        }
    }
}
