using LaboratoryApp.Domain.Models.Chemistry.CompoundFunction;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Chemistry.CompoundFunction
{
    public class CompoundProvider : ICompoundProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<Compound> _compoundCollection;

        public CompoundProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(db => db.DatabaseName == DatabaseName.ChemistryMongoDB);
            _compoundCollection = _mongoDb.GetCollection<Compound>(CollectionName.Compounds);
        }

        /// <summary>
        /// Get all compounds from the MongoDB database.
        /// </summary>
        /// <returns>All compounds</returns>
        public async Task<List<Compound>> GetAllCompoundsAsync()
        {
            var compounds = await _compoundCollection.Find(FilterDefinition<Compound>.Empty).ToListAsync();
            return compounds;
        }

        /// <summary>
        /// Add a new compound to the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public async Task AddCompoundAsync(Compound compound)
        {
            await _compoundCollection.InsertOneAsync(compound);
        }

        /// <summary>
        /// Update an existing compound in the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public async Task UpdateCompoundAsync(Compound compound)
        {
            await _compoundCollection.ReplaceOneAsync(c => c.Id == compound.Id, compound);
        }

        /// <summary>
        /// Delete a compound from the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public async Task DeleteCompoundAsync(Compound compound)
        {
            await _compoundCollection.DeleteOneAsync(c => c.Id == compound.Id);
        }
    }
}
