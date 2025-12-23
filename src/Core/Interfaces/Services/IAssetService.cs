using LaboratoryApp.Domain.Models.Core;

namespace LaboratoryApp.src.Core.Interfaces.Services
{
    public interface IAssetService
    {
        Task<Asset> GetOrAddTextAssetAsync(string content, long ownerId);
        Task<string> GetTextContentAsync(long assetId);
        Task ReleaseAssetAsync(long assetId);
    }
}
