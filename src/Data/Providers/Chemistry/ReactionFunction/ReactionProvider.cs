using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Data.Providers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Chemistry.ReactionFunction
{
    public class ReactionProvider : IReactionProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public ReactionProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.ChemistryMongoDB);
        }

        /// <summary>
        /// Get all reactions from the MongoDB database.
        /// </summary>
        /// <returns></returns>
        public List<Reaction> GetAllReactions()
        {
            return _mongoDb.GetAll<Reaction>(CollectionName.Reactions);
        }

        /// <summary>
        /// Get all reactions from the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public void AddReaction(Reaction reaction)
        {
            _mongoDb.Insert(CollectionName.Reactions, reaction);
        }

        /// <summary>
        /// Update an existing reaction in the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public void UpdateReaction(Reaction reaction)
        {
            _mongoDb.Update(CollectionName.Reactions, reaction.Id, reaction);
        }

        /// <summary>
        /// Delete a reaction from the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public void DeleteReaction(Reaction reaction)
        {
            _mongoDb.Delete<Reaction>(CollectionName.Reactions, reaction.Id); // Explicitly specify the type argument
        }
    }
}
