using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.Chemistry.ReactionFunction
{
    public interface IReactionService
    {
        IEnumerable<Reaction> GetReactionSuggestions(string Reactants, string Products);
        IEnumerable<object> GetElementCompoundSuggestions(string SearchText, SubstanceKind Kind);
        public void SaveReaction(Reaction reaction);
    }
}
