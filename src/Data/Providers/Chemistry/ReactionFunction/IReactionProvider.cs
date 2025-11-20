using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;

namespace LaboratoryApp.src.Data.Providers.Chemistry.ReactionFunction
{
    public interface IReactionProvider
    {
        Task<List<Reaction>> GetAllReactionsAsync(); // Method to get all reactions from the database
        Task AddReactionAsync(Reaction reaction); // Method to add a new reaction to the database
        Task UpdateReactionAsync(Reaction reaction); // Method to update an existing reaction in the database
        Task DeleteReactionAsync(Reaction reaction); // Method to delete a reaction from the database
    }
}
