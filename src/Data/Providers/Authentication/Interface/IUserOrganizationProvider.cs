using LaboratoryApp.Domain.Models.Users;

namespace LaboratoryApp.src.Data.Providers.Authentication.Interface
{
    public interface IUserOrganizationProvider
    {
        Task<List<UserOrganization>> GetAllUserOrganizationsAsync();
        Task<List<UserOrganization>> GetUserOrganizationsByUserIdAsync(long userId);
        Task<List<UserOrganization>> GetUserOrganizationsByOrganizationIdAsync(long organizationId);
        Task CreateUserOrganizationAsync(UserOrganization userOrganization);
        Task UpdateUserOrganizationAsync(UserOrganization userOrganization);
        Task DeleteUserOrganizationAsync(long userOrganizationId);
    }
}
