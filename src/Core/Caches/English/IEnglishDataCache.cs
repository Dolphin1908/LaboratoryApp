using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Data.Providers.English.DiaryFunction;
using LaboratoryApp.src.Data.Providers.English.DictionaryFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
