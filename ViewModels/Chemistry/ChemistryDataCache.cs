using LaboratoryApp.Models.Chemistry;
using LaboratoryApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.ViewModels.Chemistry
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
            }
        }
    }
}
