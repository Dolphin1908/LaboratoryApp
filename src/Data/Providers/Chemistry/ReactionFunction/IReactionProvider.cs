using LaboratoryApp.src.Core.Models.Chemistry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Chemistry.ReactionFunction
{
    public interface IReactionProvider
    {
        public List<Reaction> GetAllReactions(); // Method to get all reactions from the database
        public void AddReaction(Reaction reaction); // Method to add a new reaction to the database
        public void UpdateReaction(Reaction reaction); // Method to update an existing reaction in the database
        public void DeleteReaction(Reaction reaction); // Method to delete a reaction from the database
    }
}
