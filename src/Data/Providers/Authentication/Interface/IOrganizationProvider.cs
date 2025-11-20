using LaboratoryApp.Domain.Models.Users;

namespace LaboratoryApp.src.Data.Providers.Authentication.Interface
{
    public interface IOrganizationProvider
    {
        Task<List<Organization>> GetAllOrganizationsAsync();
        Task<List<Organization>?> GetOrganizationByNameAsync(string organizationName);
        Task<Organization?> GetOrganizationByIdAsync(long organizationId);
        Task CreateOrganizationAsync(Organization organization);
        Task UpdateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(long organizationId);
    }
}
