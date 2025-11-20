using LaboratoryApp.Domain.DTOs.Authentication;
using LaboratoryApp.Domain.DTOs.Users;

namespace LaboratoryApp.src.Services.Authentication.Common
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAsync(string username, string password, string confirmPassword, string email, string phoneNumber);
        Task<LoginResultDTO> AuthenticateAsync(string username, string password);
        void SetSession(LoginResultDTO loginResultDTO, UserOrganizationProfileDTO userOrganizationDTO);
    }
}
