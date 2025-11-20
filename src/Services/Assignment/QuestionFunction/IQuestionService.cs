using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Services.Assignment.QuestionFunction
{
    public interface IQuestionService
    {
        QuestionBaseViewModel Create(QuestionType type);
    }
}
