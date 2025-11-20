using LaboratoryApp.Domain.Models.Users;

namespace LaboratoryApp.src.Data.Providers.Authentication.Interface
{
    public interface IRefreshTokenProvider
    {
        Task CreateAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task UpdateAsync(RefreshToken token);
        Task<RefreshToken?> GetLatestByUserIdAsync(long userId);
    }
}
