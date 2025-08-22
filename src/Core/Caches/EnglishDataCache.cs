using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Services.English;

namespace LaboratoryApp.src.Core.Caches
{
    public class EnglishDataCache
    {
        private readonly object _lock = new();
        public bool IsLoaded { get; private set; } = false;

        public List<Word> AllWords { get; set; } = new List<Word>();
        public List<Pos> AllPos { get; set; } = new List<Pos>();
        public List<Definition> AllDefinitions { get; set; } = new List<Definition>();
        public List<Example> AllExamples { get; set; } = new List<Example>();


        public void LoadAllData(IEnglishService service)
        {
            if (IsLoaded) return;
            lock (_lock)
            {
                if (IsLoaded) return; // Double-check after acquiring the lock

                AllWords = service.GetAllWords();
                AllPos = service.GetAllPos();
                AllDefinitions = service.GetAllDefinitions();
                AllExamples = service.GetAllExamples();

                IsLoaded = true;
            }
        }
    }
}
