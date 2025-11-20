using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Chemistry.ReactionFunction
{
    public class ReactionProvider : IReactionProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<Reaction> _reactionCollection;
        public ReactionProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.ChemistryMongoDB);
            _reactionCollection = _mongoDb.GetCollection<Reaction>(CollectionName.Reactions);
        }

        /// <summary>
        /// Get all reactions from the MongoDB database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Reaction>> GetAllReactionsAsync()
        {
            var reactions = await _reactionCollection.Find(FilterDefinition<Reaction>.Empty).ToListAsync();
            return reactions;
        }

        /// <summary>
        /// Get all reactions from the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public async Task AddReactionAsync(Reaction reaction)
        {
            await _reactionCollection.InsertOneAsync(reaction);
        }

        /// <summary>
        /// Update an existing reaction in the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public async Task UpdateReactionAsync(Reaction reaction)
        {
            await _reactionCollection.ReplaceOneAsync(r => r.Id == reaction.Id, reaction);
        }

        /// <summary>
        /// Delete a reaction from the MongoDB database.
        /// </summary>
        /// <param name="reaction"></param>
        public async Task DeleteReactionAsync(Reaction reaction)
        {
            await _reactionCollection.DeleteOneAsync(r => r.Id == reaction.Id);
        }
    }
}
