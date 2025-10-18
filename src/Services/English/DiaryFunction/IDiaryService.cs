using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LaboratoryApp.src.Services.English.DiaryFunction
{
    public interface IDiaryService
    {
        Task CreateDiaryAsync(string title, bool isPublic, FlowDocument document);
        Task<List<DiaryContent>> GetPublicDiariesAsync();
        Task<List<DiaryContent>> GetPrivateDiariesForCurrentUserAsync(long userId);
        Task UpdateDiaryAsync(DiaryContent originalDiary, string newTitle, bool newIsPublic, FlowDocument newDocument);
        Task DeleteDiaryAsync(DiaryContent diary);
    }
}
