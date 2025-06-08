using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

using LaboratoryApp.src.Core.Models.Authentication;

namespace LaboratoryApp.src.Data.Providers.Authentication.Interfaces
{
    public interface IRoleProvider
    {
        List<Role> GetAllRoles();
        Role GetRoleById(long id);
    }
}
