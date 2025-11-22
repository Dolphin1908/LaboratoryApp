using LaboratoryApp.Domain.DTOs.Users;
using LaboratoryApp.Domain.Interfaces.Providers.Users;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Interfaces.Services.Users;
using LaboratoryApp.Domain.Models.Users;
using LaboratoryApp.src.Constants;

namespace LaboratoryApp.src.Services.Users
{
    public class UserOrganizationService : IUserOrganizationService
    {
        private readonly IUserOrganizationProvider _userOrganizationProvider;

        private readonly ICounterService _counterService;
        private readonly IOrganizationService _organizationService;

        public UserOrganizationService(IUserOrganizationProvider userOrganizationProvider,
                                       ICounterService counterService,
                                       IOrganizationService organizationService)
        {
            _userOrganizationProvider = userOrganizationProvider;
            _counterService = counterService;
            _organizationService = organizationService;
        }

        public async Task<List<UserOrganizationProfileDTO>> GetUserOrganizationDTOsByUserIdAsync(long userId)
        {
            var userOrganizations = await GetUserOrganizationsByUserIdAsync(userId);
            List<UserOrganizationProfileDTO> results = new List<UserOrganizationProfileDTO>();

            foreach (var userOrganization in userOrganizations)
            {
                var temp = await _organizationService.GetOrganizationByIdAsync(userOrganization.OrganizationId);
                results.Add(new UserOrganizationProfileDTO
                {
                    OrganizationId = userOrganization.OrganizationId,
                    OrganizationName = temp!.Name,
                    Role = userOrganization.Role
                });
            }

            return results;
        }

        public async Task<List<UserOrganization>> GetAllUserOrganizationsAsync()
        {
            var userOrganizations = _userOrganizationProvider.GetAllUserOrganizationsAsync();
            return userOrganizations.GetAwaiter().GetResult();
        }

        public async Task<List<UserOrganization>> GetUserOrganizationsByUserIdAsync(long userId)
        {
            var userOrganizations = await _userOrganizationProvider.GetUserOrganizationsByUserIdAsync(userId);
            return userOrganizations;
        }

        public async Task<List<UserOrganization>> GetUserOrganizationsByOrganizationIdAsync(long organizationId)
        {
            var userOrganizations = _userOrganizationProvider.GetUserOrganizationsByOrganizationIdAsync(organizationId);
            return userOrganizations.GetAwaiter().GetResult();
        }

        public async Task CreateUserOrganizationAsync(UserOrganization userOrganization)
        {
            userOrganization.Id = _counterService.GetNextId(CollectionName.UserOrganization);
            await _userOrganizationProvider.CreateUserOrganizationAsync(userOrganization);
        }

        public async Task UpdateUserOrganizationAsync(UserOrganization userOrganization)
        {
            userOrganization.UpdatedAt = DateTime.UtcNow;
            await _userOrganizationProvider.UpdateUserOrganizationAsync(userOrganization);
        }

        public async Task DeleteUserOrganizationAsync(UserOrganization userOrganization)
        {
            await _userOrganizationProvider.DeleteUserOrganizationAsync(userOrganization.Id);
        }
    }
}
