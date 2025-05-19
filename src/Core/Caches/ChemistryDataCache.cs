using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Services.Chemistry;

namespace LaboratoryApp.src.Core.Caches
{
    public class ChemistryDataCache
    {
        public static List<Element> AllElements { get; set; }
        public static List<Compound> AllCompounds { get; set; }
        public static List<Reaction> AllReactions { get; set; }

        public static bool IsLoaded => AllElements != null;

        public static void LoadAllData(ChemistryService service)
        {
            if (!IsLoaded)
            {
                AllElements = service.GetAllElements();
                AllCompounds = service.GetAllCompounds();
                AllReactions = service.GetAllReactions();
            }
        }
    }
}
