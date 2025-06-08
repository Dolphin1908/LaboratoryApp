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
    public class RoleProvider : IRoleProvider
    {
        private readonly IMongoDBProvider _db;
        public RoleProvider(IMongoDBProvider db) => _db = db;

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
