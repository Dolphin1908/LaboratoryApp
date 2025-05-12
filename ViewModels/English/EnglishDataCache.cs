using LaboratoryApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.Models.English.DictionaryFunction;

namespace LaboratoryApp.ViewModels.English.DictionaryFunction
{
    public class EnglishDataCache
    {
        public static List<Word> AllWords { get; set; }
        public static List<Pos> AllPos { get; set; }
        public static List<Definition> AllDefinitions { get; set; }
        public static List<Example> AllExamples { get; set; }

        public static bool IsLoaded => AllWords != null;

        public static void LoadAllData(EnglishService service)
        {
            if (!IsLoaded)
            {
                AllWords = service.GetAllWords();
                AllPos = service.GetAllPos();
                AllDefinitions = service.GetAllDefinitions();
                AllExamples = service.GetAllExamples();
            }
        }
    }
}
