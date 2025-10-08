using LaboratoryApp.src.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Authentication.Interfaces
{
    public interface IUserRoleProvider
    {
        Task<UserRole?> GetUserRolesAsync(long userId);
        Task CreateUserRole(UserRole userRole);
        long GetNextUserRoleId();
    }
}
