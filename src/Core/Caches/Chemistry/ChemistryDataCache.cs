using LaboratoryApp.Domain.Interfaces.Providers.Chemistry;
using LaboratoryApp.Domain.Models.Chemistry.Common;
using LaboratoryApp.Domain.Models.Chemistry.CompoundFunction;
using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;

namespace LaboratoryApp.src.Core.Caches.Chemistry
{
    public class ChemistryDataCache : IChemistryDataCache
    {
        private readonly object _lock = new();

        public List<Element> AllElements { get; set; } = new();
        public List<Compound> AllCompounds { get; set; } = new();
        public List<Reaction> AllReactions { get; set; } = new();

        public void LoadAllData(IPeriodicProvider periodicProvider,
                                ICompoundProvider compoundProvider,
                                IReactionProvider reactionProvider)
        {
            lock (_lock)
            {
                LoadAllDataAsync(periodicProvider, compoundProvider, reactionProvider);
            }
        }

        private async void LoadAllDataAsync(IPeriodicProvider periodicProvider,
                                            ICompoundProvider compoundProvider,
                                            IReactionProvider reactionProvider)
        {
            AllElements = await periodicProvider.GetAllElementsAsync();
            AllCompounds = await compoundProvider.GetAllCompoundsAsync();
            AllReactions = await reactionProvider.GetAllReactionsAsync();

            foreach (var reaction in AllReactions)
            {
                foreach (var reactant in reaction.Reactants)
                {
                    reactant.Formula = AllElements.FirstOrDefault(e => e.Id == reactant.ElementId)?.Formula ?? AllCompounds.FirstOrDefault(e => e.Id == reactant.CompoundId)?.Formula ?? string.Empty;
                    reactant.DisplayCoefficient = reactant.Coefficient == "1" ? string.Empty : reactant.Coefficient.ToString();
                    reactant.Display = string.IsNullOrEmpty(reactant.DisplayCoefficient) ? reactant.Formula : $"{reactant.DisplayCoefficient} {reactant.Formula}";
                }

                foreach (var product in reaction.Products)
                {
                    product.Formula = AllElements.FirstOrDefault(e => e.Id == product.ElementId)?.Formula ?? AllCompounds.FirstOrDefault(e => e.Id == product.CompoundId)?.Formula ?? string.Empty;
                    product.DisplayCoefficient = product.Coefficient == "1" ? string.Empty : product.Coefficient.ToString();
                    product.Display = string.IsNullOrEmpty(product.DisplayCoefficient) ? product.Formula : $"{product.DisplayCoefficient} {product.Formula}";
                }
            }
        }
    }
}
