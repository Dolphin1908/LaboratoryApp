using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Data.Providers.Assignment.QuestionFunction;

namespace LaboratoryApp.src.Services.Assignment.QuestionFunction
{
    public class QuestionService : IQuestionService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IQuestionProvider _questionProvider;

        public QuestionService(IServiceProvider serviceProvider,
                               IQuestionProvider questionProvider)
        {
            _serviceProvider = serviceProvider;
            _questionProvider = questionProvider;
        }

        public QuestionBaseViewModel Create(QuestionType type)
        {
            return type switch
            {

                _ => throw new ArgumentException("Unsupported question type")
            };
        }
    }
}
