using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;

namespace LaboratoryApp.src.Data.Providers.Authentication
{
    public class RoleProvider
    {
        private readonly MongoDBProvider _db;
        public RoleProvider(MongoDBProvider db) => _db = db;

        public List<Role> GetAllRoles()
        {
            var roles = _db.GetAll<Role>("roles");
            return roles;
        }

        public Role GetRoleById(long id)
        {
            var filter = Builders<Role>.Filter.Eq(r => r.Id, id);
            var role = _db.GetOne("roles", filter);
            return role;
        }
    }
}
