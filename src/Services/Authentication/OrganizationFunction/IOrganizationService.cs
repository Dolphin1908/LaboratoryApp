using LaboratoryApp.Domain.Models.Users;

namespace LaboratoryApp.src.Services.Authentication.OrganizationFunction
{
    public interface IOrganizationService
    {
        Task<Organization?> GetOrganizationByIdAsync(long organizationId);
        Task CreateOrganizationAsync(Organization organization);
        Task UpdateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(Organization organization);
    }
}
