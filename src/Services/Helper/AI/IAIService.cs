using LaboratoryApp.Domain.DTOs.English.DiaryFunction;
using LaboratoryApp.Domain.DTOs.English.DictionaryFunction;

namespace LaboratoryApp.src.Services.Helper.AI
{
    public interface IAIService
    {
        Task<WordResultDTO?> SearchWordWithAIAsync(string word);
        Task<DiaryResultDTO?> EditDiaryWithAIAsync(string title, string body);
    }
}
