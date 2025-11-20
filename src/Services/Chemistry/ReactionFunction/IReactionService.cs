using LaboratoryApp.Domain.Enums.Chemistry.ReactionFunction;
using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;

namespace LaboratoryApp.src.Services.Chemistry.ReactionFunction
{
    public interface IReactionService
    {
        IEnumerable<Reaction> GetReactionSuggestions(string Reactants, string Products);
        IEnumerable<object> GetElementCompoundSuggestions(string SearchText, SubstanceKind Kind);
        public void SaveReaction(Reaction reaction);
    }
}
