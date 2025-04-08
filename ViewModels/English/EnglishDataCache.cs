using LaboratoryApp.Models.English;
using LaboratoryApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.ViewModels.English
{
    public class EnglishDataCache
    {
        public static List<WordModel> AllWords { get; set; }
        public static List<PosModel> AllPos { get; set; }
        public static List<DefinitionModel> AllDefinitions { get; set; }
        public static List<ExampleModel> AllExamples { get; set; }

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
