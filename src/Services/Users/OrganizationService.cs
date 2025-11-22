using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Interfaces.Services.Users;
using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Constants;

namespace LaboratoryApp.src.Services.Users
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationProvider _organizationProvider;

        private readonly ICounterService _counterService;

        public OrganizationService(IOrganizationProvider organizationProvider,
                                   ICounterService counterService)
        {
            _organizationProvider = organizationProvider;
            _counterService = counterService;
        }

        public async Task<Organization?> GetOrganizationByIdAsync(long organizationId)
        {
            // Await the async method and handle nullable return
            var organizationTask = await _organizationProvider.GetOrganizationByIdAsync(organizationId);
            return organizationTask;
        }

        public async Task CreateOrganizationAsync(Organization organization)
        {
            organization.Id = _counterService.GetNextId(CollectionName.Organizations);
            await _organizationProvider.CreateOrganizationAsync(organization);
        }

        public async Task UpdateOrganizationAsync(Organization organization)
        {
            await _organizationProvider.UpdateOrganizationAsync(organization);
        }

        public async Task DeleteOrganizationAsync(Organization organization)
        {
            await _organizationProvider.DeleteOrganizationAsync(organization.Id);
        }
    }
}
