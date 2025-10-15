using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;
using LaboratoryApp.src.Data.Providers.Common;
using LaboratoryApp.src.Data.Providers.Authentication.Interfaces;
using LaboratoryApp.src.Constants;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class RoleProvider : IRoleProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        public RoleProvider(IEnumerable<IMongoDBProvider> mongoDb) => _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AuthenticationMongoDB);

        public List<Role> GetAllRoles()
        {
            var roles = _mongoDb.GetAll<Role>(CollectionName.Roles);
            return roles;
        }

        public Role GetRoleById(long id)
        {
            var filter = Builders<Role>.Filter.Eq(r => r.Id, id);
            var role = _mongoDb.GetOne(CollectionName.Roles, filter);
            return role;
        }
    }
}
