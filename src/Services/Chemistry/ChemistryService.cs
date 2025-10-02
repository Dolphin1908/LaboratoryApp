using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Data.Providers;
using LaboratoryApp.src.Data.Providers.Interfaces;

namespace LaboratoryApp.src.Services.Chemistry
{
    public class ChemistryService : IChemistryService
    {
        private readonly ISQLiteDataProvider _sqliteDb;
        private readonly IMongoDBProvider _mongoDb;

        public ChemistryService(IEnumerable<ISQLiteDataProvider> sqliteDb,
                                IEnumerable<IMongoDBProvider> mongoDb)
        {
            _sqliteDb = sqliteDb.First(d => d.DatabaseName == DatabaseName.ChemistrySQLite);
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.ChemistryMongoDB);
        }

        #region SQLite
        /// <summary>
        /// Get all elements from the SQLite database.
        /// </summary>
        /// <returns>All elements</returns>
        public List<Element> GetAllElements()
        {
            try
            {
                return _sqliteDb.ExecuteQuery<Element>($"SELECT * FROM {CollectionName.Elements}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching elements: {ex.Message}");
                return new List<Element>();
            }
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
                return _mongoDb.GetAll<Compound>(CollectionName.Compounds);
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
                _mongoDb.Insert(CollectionName.Compounds, compound);
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
                _mongoDb.Update(CollectionName.Compounds, compound.Id, compound);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the compound: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Delete a compound from the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void DeleteCompound(Compound compound)
        {
            try
            {
                _mongoDb.Delete<Compound>(CollectionName.Compounds, compound.Id); // Explicitly specify the type argument
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
                return _mongoDb.GetAll<Reaction>(CollectionName.Reactions);
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
                _mongoDb.Insert(CollectionName.Reactions, reaction);
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
                _mongoDb.Update(CollectionName.Reactions, reaction.Id, reaction);
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
                _mongoDb.Delete<Reaction>(CollectionName.Reactions, reaction.Id); // Explicitly specify the type argument
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
