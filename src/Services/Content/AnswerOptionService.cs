using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Interfaces.Services;

namespace LaboratoryApp.src.Services.Content
{
    public class AnswerOptionService : IAnswerOptionService
    {
        private readonly IAnswerOptionProvider _answerOptionProvider;
        private readonly IAssetService _assetService;

        public AnswerOptionService(IAnswerOptionProvider answerOptionProvider,
                                   IAssetService assetService)
        {
            _answerOptionProvider = answerOptionProvider;
            _assetService = assetService;
        }

        public async Task<List<AnswerOption>> GetAnswerOptionsByQuestionIdAsync(long questionId)
        {
            var answerOptions = await _answerOptionProvider.GetAllAnswerOptionsAsync();
            var results = answerOptions.Where(ans => ans.QuestionId == questionId)
                                      .OrderBy(x => x.OrderIndex)
                                      .ToList();

            foreach (var result in results)
            {
                result.Content = await _assetService.GetTextContentAsync(result.AssetId);
            }

            return results;
        }

        /// <summary>
        /// Lưu mới một tùy chọn trả lời
        /// </summary>
        /// <param name="newAnswerOption"></param>
        /// <returns></returns>
        public async Task SaveNewAnswerOptionAsync(AnswerOption newAnswerOption)
        {
            await _answerOptionProvider.CreateNewAnswerOptionAsync(newAnswerOption);
        }
    }
}
