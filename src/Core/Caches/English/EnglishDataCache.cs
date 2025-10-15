using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Data.Providers.English.DiaryFunction;
using LaboratoryApp.src.Data.Providers.English.DictionaryFunction;

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
                AllDiaries = diaryProvider.GetAllDiaries();

                AllWords = dictionaryProvider.GetAllWords();
                AllPos = dictionaryProvider.GetAllPos();
                AllDefinitions = dictionaryProvider.GetAllDefinitions();
                AllExamples = dictionaryProvider.GetAllExamples();
            }
        }
    }
}
