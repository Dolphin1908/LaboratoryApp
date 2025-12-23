using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Interfaces.Services;

namespace LaboratoryApp.src.Services.Content
{
    public class QuestionBlockService : IQuestionBlockService
    {
        private readonly IQuestionBlockProvider _questionBlockProvider;
        private readonly IAssetService _assetService;
        private readonly IQuestionService _questionService;

        public QuestionBlockService(IQuestionBlockProvider questionBlockProvider,
                                    IAssetService assetService,
                                    IQuestionService questionService)
        {
            _questionBlockProvider = questionBlockProvider;
            _assetService = assetService;
            _questionService = questionService;
        }

        public async Task<List<QuestionBlock>> GetQuestionBlocksByExerciseIdAsync(long exerciseId)
        {
            var allQuestionBlocks = await _questionBlockProvider.GetAllQuestionBlocksAsync();
            var results = allQuestionBlocks.Where(qb => qb.RootExerciseId == exerciseId)
                                           .OrderBy(x => x.OrderIndex)
                                           .ToList();

            foreach (var result in results)
            {
                result.Content = await _assetService.GetTextContentAsync(result.AssetId);
                result.Questions = await _questionService.GetQuestionsByQuestionBlockIdAsync(result.Id);
            }

            return results;
        }

        public async Task SaveNewQuestionBlockAsync(QuestionBlock newQuestionBlock)
        {
            await _questionBlockProvider.CreateNewQuestionBlockAsync(newQuestionBlock);

            foreach (var question in newQuestionBlock.Questions)
            {
                question.QuestionBlockId = newQuestionBlock.Id;
                await _questionService.SaveNewQuestionAsync(question);
            }
        }
    }
}
