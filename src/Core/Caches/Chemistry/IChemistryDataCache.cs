using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Chemistry;

using LaboratoryApp.src.Data.Providers.Chemistry.CompoundFunction;
using LaboratoryApp.src.Data.Providers.Chemistry.PeriodicFunction;
using LaboratoryApp.src.Data.Providers.Chemistry.ReactionFunction;

namespace LaboratoryApp.src.Core.Caches.Chemistry
{
    public interface IChemistryDataCache
    {
        List<Element> AllElements { get; set; }
        List<Compound> AllCompounds { get; set; }
        List<Reaction> AllReactions { get; set; }

        void LoadAllData(IPeriodicProvider periodicProvider,
                         ICompoundProvider compoundProvider,
                         IReactionProvider reactionProvider);
    }
}
