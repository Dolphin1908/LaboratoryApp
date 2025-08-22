using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry;

namespace LaboratoryApp.src.Core.Caches
{
    public class ChemistryDataCache
    {
        private readonly object _lock = new();
        public bool IsLoaded { get; private set; } = false;

        public List<Element> AllElements { get; set; } = new();
        public List<Compound> AllCompounds { get; set; } = new();
        public List<Reaction> AllReactions { get; set; } = new();

        public void LoadAllData(IChemistryService service)
        {
            if(IsLoaded) return;

            lock (_lock)
            {
                if (IsLoaded) return; // Double-check after acquiring the lock

                AllElements = service.GetAllElements();
                AllCompounds = service.GetAllCompounds();
                AllReactions = service.GetAllReactions();

                IsLoaded = true;
            }
        }
    }
}
