using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Constants;

using LaboratoryApp.src.Core.Models.Chemistry;

using LaboratoryApp.src.Data.Providers.Common;

namespace LaboratoryApp.src.Data.Providers.Chemistry.CompoundFunction
{
    public class CompoundProvider : ICompoundProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public CompoundProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(db => db.DatabaseName == DatabaseName.ChemistryMongoDB);
        }

        /// <summary>
        /// Get all compounds from the MongoDB database.
        /// </summary>
        /// <returns>All compounds</returns>
        public List<Compound> GetAllCompounds()
        {
            return _mongoDb.GetAll<Compound>(CollectionName.Compounds);
        }

        /// <summary>
        /// Add a new compound to the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void AddCompound(Compound compound)
        {
            _mongoDb.Insert(CollectionName.Compounds, compound);
        }

        /// <summary>
        /// Update an existing compound in the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void UpdateCompound(Compound compound)
        {
            _mongoDb.Update(CollectionName.Compounds, compound.Id, compound);
        }

        /// <summary>
        /// Delete a compound from the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void DeleteCompound(Compound compound)
        {
            _mongoDb.Delete<Compound>(CollectionName.Compounds, compound.Id); // Explicitly specify the type argument
        }
    }
}
