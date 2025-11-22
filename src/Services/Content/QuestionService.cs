using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Interfaces.Services.Content;

namespace LaboratoryApp.src.Services.Content
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
    }
}
