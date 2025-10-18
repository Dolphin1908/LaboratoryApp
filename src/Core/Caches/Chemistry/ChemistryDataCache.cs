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
                AllElements = periodicProvider.GetAllElements();
                AllCompounds = compoundProvider.GetAllCompounds();
                AllReactions = reactionProvider.GetAllReactions();

                foreach(var reaction in AllReactions)
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
}
