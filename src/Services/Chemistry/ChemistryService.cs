using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Data.Providers;

namespace LaboratoryApp.src.Services.Chemistry
{
    public class ChemistryService : IChemistryService
    {
        private readonly string _chemDbPath = ConfigurationManager.AppSettings["ChemistryDbPath"];
        private readonly string _mongoDbPath = SecureConfigHelper.Decrypt(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);

        #region SQLite
        /// <summary>
        /// Get all elements from the SQLite database.
        /// </summary>
        /// <returns>All elements</returns>
        public List<Element> GetAllElements()
        {
            using var db = new SQLiteDataProvider(_chemDbPath);
            return db.ExecuteQuery<Element>("SELECT * FROM Elements");
        }
        #endregion

        #region CompoundMongoDB
        /// <summary>
        /// Get all compounds from the MongoDB database.
        /// </summary>
        /// <returns>All compounds</returns>
        public List<Compound> GetAllCompounds()
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                return db.GetAll<Compound>("compounds");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                MessageBox.Show($"An error occurred while fetching compounds: {ex.Message}");
                return new List<Compound>();
            }
        }

        /// <summary>
        /// Add a new compound to the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void AddCompound(Compound compound)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                db.Insert("compounds", compound);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding a compound: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Update an existing compound in the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void UpdateCompound(Compound compound)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                db.Update("compounds", compound.Id, compound);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the compound: {ex.Message}");
                return;
            }
        }

        public void DeleteCompound(Compound compound)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                db.Delete<Compound>("compounds", compound.Id); // Explicitly specify the type argument
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the compound: {ex.Message}");
                return;
            }
        }
        #endregion

        #region ReactionMongoDB
        /// <summary>
        /// Get all reactions from the MongoDB database.
        /// </summary>
        /// <returns></returns>
        public List<Reaction> GetAllReactions()
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                return db.GetAll<Reaction>("reactions");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching reactions: {ex.Message}");
                return new List<Reaction>();
            }
        }

        /// <summary>
        /// Get all reactions from the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public void AddReaction(Reaction reaction)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                db.Insert("reactions", reaction);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding a reaction: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Update an existing reaction in the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public void UpdateReaction(Reaction reaction)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                db.Update("reactions", reaction.Id, reaction);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the reaction: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Delete a reaction from the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public void DeleteReaction(Reaction reaction)
        {
            try
            {
                using var db = new MongoDBProvider(_mongoDbPath, "chemistry");
                db.Delete<Reaction>("reactions", reaction.Id); // Explicitly specify the type argument
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the reaction: {ex.Message}");
                return;
            }
        }
        #endregion
    }
}
