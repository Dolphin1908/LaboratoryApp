using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Services.English;

namespace LaboratoryApp.src.Core.Caches
{
    public static class EnglishDataCache
    {
        private static readonly object _lock = new();
        public static bool IsLoaded { get; private set; } = false;

        public static List<DiaryContent> AllDiaries { get; set; } = new List<DiaryContent>();

        public static List<Word> AllWords { get; set; } = new List<Word>();
        public static List<Pos> AllPos { get; set; } = new List<Pos>();
        public static List<Definition> AllDefinitions { get; set; } = new List<Definition>();
        public static List<Example> AllExamples { get; set; } = new List<Example>();


        public static void LoadAllData(IEnglishService service)
        {
            if (IsLoaded) return;
            lock (_lock)
            {
                if (IsLoaded) return; // Double-check after acquiring the lock

                AllDiaries = service.GetAllDiaries();

                AllWords = service.GetAllWords();
                AllPos = service.GetAllPos();
                AllDefinitions = service.GetAllDefinitions();
                AllExamples = service.GetAllExamples();

                IsLoaded = true;
            }
        }
    }
}
