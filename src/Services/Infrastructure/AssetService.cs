using LaboratoryApp.Domain.Enums.Core;
using LaboratoryApp.Domain.Interfaces.Providers.Infrastructure;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.Core;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Interfaces.Services;

namespace LaboratoryApp.src.Services.Infrastructure
{
    /// <summary>
    /// Xử lý các thao tác liên quan đến tài nguyên (Asset)
    /// </summary>
    public class AssetService : IAssetService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IFormatConversionService _formatConversionService;

        public AssetService(IAssetProvider assetProvider,
                            IFormatConversionService formatConversionService)
        {
            _assetProvider = assetProvider;
            _formatConversionService = formatConversionService;
        }

        /// <summary>
        /// Lấy hoặc thêm mới tài nguyên văn bản
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<Asset> GetOrAddTextAssetAsync(string content, long ownerId)
        {
            if (content == null)
                content = string.Empty;

            // LOGIC: Tính hash
            string hash = AssetHashHelper.ComputeTextHash(content);

            // LOGIC: Kiểm tra tồn tại
            var existingAsset = await _assetProvider.GetByHashAsync(hash, AssetType.TextContent);

            if (existingAsset != null)
            {
                existingAsset.ReferenceCount++;
                await _assetProvider.UpdateAssetAsync(existingAsset);
                return existingAsset;
            }

            var newAsset = new Asset
            {
                Type = AssetType.TextContent,
                Content = _formatConversionService.SerializeStringToByteArray(content),
                HashValue = hash,
                ReferenceCount = 1,
                OwnerUserId = ownerId,
                CreatedAt = DateTime.Now
            };

            await _assetProvider.AddAssetAsync(newAsset);
            return newAsset;
        }

        /// <summary>
        /// Lấy nội dung văn bản từ tài nguyên (Asset)
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public async Task<string> GetTextContentAsync(long assetId)
        {
            var asset = await _assetProvider.GetByIdAsync(assetId);
            if (asset == null || asset.Content == null)
                return string.Empty;

            return _formatConversionService.DeserializeStringFromByteArray(asset.Content);
        }

        /// <summary>
        /// Giảm số lượng tham chiếu của tài nguyên (Asset) và xóa nếu không còn tham chiếu nào
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public async Task ReleaseAssetAsync(long assetId)
        {
            var asset = await _assetProvider.GetByIdAsync(assetId);
            if (asset == null) return;

            // LOGIC: Giảm ref
            asset.ReferenceCount--;

            if (asset.ReferenceCount <= 0)
            {
                await _assetProvider.DeleteAssetAsync(asset);
            }
            else
            {
                await _assetProvider.UpdateAssetAsync(asset);
            }
        }
    }
}
