using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Interfaces.Services;

namespace LaboratoryApp.src.Services.Content
{
    public class QuestionService : IQuestionService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IQuestionProvider _questionProvider;
        private readonly IAnswerOptionService _answerOptionService;
        private readonly IAssetService _assetService;

        public QuestionService(IServiceProvider serviceProvider,
                               IQuestionProvider questionProvider,
                               IAnswerOptionService answerOptionService,
                               IAssetService assetService)
        {
            _serviceProvider = serviceProvider;
            _questionProvider = questionProvider;
            _answerOptionService = answerOptionService;
            _assetService = assetService;
        }

        public async Task<List<Question>> GetQuestionsByQuestionBlockIdAsync(long questionBlockId)
        {
            var allQuestions = await _questionProvider.GetAllQuestionsAsync();
            var results = allQuestions.Where(q => q.QuestionBlockId == questionBlockId)
                                      .OrderBy(x => x.OrderIndex)
                                      .ToList();

            foreach (var result in results)
            {
                result.Title = await _assetService.GetTextContentAsync(result.AssetId);
                result.AnswerOptions = await _answerOptionService.GetAnswerOptionsByQuestionIdAsync(result.Id);
            }

            return results;
        }

        public async Task<List<Question>> GetQuestionsByExerciseIdAsync(long exerciseId)
        {
            var allQuestions = await _questionProvider.GetAllQuestionsAsync();
            var results = allQuestions.Where(q => q.RootExerciseId == exerciseId)
                                      .OrderBy(x => x.OrderIndex)
                                      .ToList();

            foreach (var result in results)
            {
                result.AnswerOptions = await _answerOptionService.GetAnswerOptionsByQuestionIdAsync(result.Id);
            }

            return results;
        }

        public async Task SaveNewQuestionAsync(Question newQuestion)
        {
            await _questionProvider.CreateNewQuestionAsync(newQuestion);

            foreach (var answerOption in newQuestion.AnswerOptions)
            {
                answerOption.QuestionId = newQuestion.Id;
                await _answerOptionService.SaveNewAnswerOptionAsync(answerOption);
            }
        }
    }
}
