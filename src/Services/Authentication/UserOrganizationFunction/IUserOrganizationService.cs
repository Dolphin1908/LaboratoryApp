using LaboratoryApp.Domain.DTOs.Users;
using LaboratoryApp.Domain.Models.Users;

namespace LaboratoryApp.src.Services.Authentication.UserOrganizationFunction
{
    public interface IUserOrganizationService
    {
        Task<List<UserOrganization>> GetAllUserOrganizationsAsync();
        Task<List<UserOrganization>> GetUserOrganizationsByUserIdAsync(long userId);
        Task<List<UserOrganizationProfileDTO>> GetUserOrganizationDTOsByUserIdAsync(long userId);
        Task<List<UserOrganization>> GetUserOrganizationsByOrganizationIdAsync(long organizationId);
        Task CreateUserOrganizationAsync(UserOrganization userOrganization);
        Task UpdateUserOrganizationAsync(UserOrganization userOrganization);
        Task DeleteUserOrganizationAsync(UserOrganization userOrganization);
    }
}
