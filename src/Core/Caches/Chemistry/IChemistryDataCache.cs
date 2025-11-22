using LaboratoryApp.Domain.Interfaces.Providers.Chemistry;
using LaboratoryApp.Domain.Models.Chemistry.Common;
using LaboratoryApp.Domain.Models.Chemistry.CompoundFunction;
using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;

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
