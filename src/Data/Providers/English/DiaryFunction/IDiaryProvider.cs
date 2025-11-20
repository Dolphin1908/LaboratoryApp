using LaboratoryApp.Domain.Models.English.DiaryFunction;

namespace LaboratoryApp.src.Data.Providers.English.DiaryFunction
{
    public interface IDiaryProvider
    {
        Task AddDiaryAsync(DiaryContent diary);
        Task<List<DiaryContent>> GetAllDiariesAsync();
        Task UpdateDiaryAsync(DiaryContent diary);
        Task DeleteDiaryAsync(long id);
    }
}
