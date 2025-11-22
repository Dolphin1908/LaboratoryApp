using LaboratoryApp.Domain.Interfaces.Providers.English;
using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.Domain.Models.English.DictionaryFunction;

namespace LaboratoryApp.src.Core.Caches.English
{
    public interface IEnglishDataCache
    {
        List<DiaryContent> AllDiaries { get; set; }

        List<Word> AllWords { get; set; }
        List<Pos> AllPos { get; set; }
        List<Definition> AllDefinitions { get; set; }
        List<Example> AllExamples { get; set; }

        void LoadAllData(IDiaryProvider diaryProvider,
                         IDictionaryProvider dictionaryProvider);
    }
}
