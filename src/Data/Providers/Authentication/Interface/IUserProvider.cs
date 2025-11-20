using LaboratoryApp.Domain.Models.Users;

namespace LaboratoryApp.src.Data.Providers.Authentication.Interface
{
    public interface IUserProvider
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetUserByIdAsync(long id);
        Task CreateNewUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
    }
}
