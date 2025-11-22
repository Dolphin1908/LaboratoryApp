using LaboratoryApp.Domain.Interfaces.Providers.English;
using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.Domain.Models.English.DictionaryFunction;

namespace LaboratoryApp.src.Core.Caches.English
{
    public class EnglishDataCache : IEnglishDataCache
    {
        private readonly object _lock = new();

        public List<DiaryContent> AllDiaries { get; set; } = new List<DiaryContent>();

        public List<Word> AllWords { get; set; } = new List<Word>();
        public List<Pos> AllPos { get; set; } = new List<Pos>();
        public List<Definition> AllDefinitions { get; set; } = new List<Definition>();
        public List<Example> AllExamples { get; set; } = new List<Example>();

        public void LoadAllData(IDiaryProvider diaryProvider,
                                IDictionaryProvider dictionaryProvider)
        {
            lock (_lock)
            {
                LoadAllDataAsync(diaryProvider, dictionaryProvider);
            }
        }

        private async void LoadAllDataAsync(IDiaryProvider diaryProvider,
                                            IDictionaryProvider dictionaryProvider)
        {
            AllDiaries = await diaryProvider.GetAllDiariesAsync();
            AllWords = await dictionaryProvider.GetAllWordsAsync();
            AllPos = await dictionaryProvider.GetAllPosAsync();
            AllDefinitions = await dictionaryProvider.GetAllDefinitionsAsync();
            AllExamples = await dictionaryProvider.GetAllExamplesAsync();
        }
    }
}
