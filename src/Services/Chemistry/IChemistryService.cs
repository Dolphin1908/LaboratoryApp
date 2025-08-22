using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Chemistry;

namespace LaboratoryApp.src.Services.Chemistry
{
    public interface IChemistryService
    {
        public List<Element> GetAllElements(); // Method to get all elements from the database
        public List<Compound> GetAllCompounds(); // Method to get all compounds from the database
        public void AddCompound(Compound compound); // Method to add a new compound to the database
        public void UpdateCompound(Compound compound); // Method to update an existing compound in the database
        public void DeleteCompound(Compound compound); // Method to delete a compound from the database
        public List<Reaction> GetAllReactions(); // Method to get all reactions from the database
        public void AddReaction(Reaction reaction); // Method to add a new reaction to the database
        public void UpdateReaction(Reaction reaction); // Method to update an existing reaction in the database
        public void DeleteReaction(Reaction reaction); // Method to delete a reaction from the database
    }
}
