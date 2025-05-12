using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.Database.Provider;
using LaboratoryApp.Models.Chemistry;
using LaboratoryApp.Support.Helpers;

namespace LaboratoryApp.Services
{
    public class ChemistryService
    {
        private readonly string _chemDbPath = ConfigurationManager.AppSettings["ChemistryDbPath"];
        private readonly string _chemMongoDbPath = "mongodb+srv://admin:19082003@cluster.st2nwet.mongodb.net/?retryWrites=true&w=majority&appName=Cluster";

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

        #region MongoDB
        /// <summary>
        /// Get all compounds from the MongoDB database.
        /// </summary>
        /// <returns>All compounds</returns>
        public List<Compound> GetAllCompounds()
        {
            using var db = new MongoDBProvider(_chemMongoDbPath, "chemistry");
            return db.GetAll<Compound>("compounds");
        }

        /// <summary>
        /// Add a new compound to the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void AddCompound(Compound compound)
        {
            using var db = new MongoDBProvider(_chemMongoDbPath, "chemistry");
            db.Insert<Compound>("compounds", compound);
        }

        /// <summary>
        /// Update an existing compound in the MongoDB database.
        /// </summary>
        /// <param name="compound"></param>
        public void UpdateCompound(Compound compound)
        {
            using var db = new MongoDBProvider(_chemMongoDbPath, "chemistry");
            db.Update<Compound>("compounds", compound.Id, compound);
        }

        public void DeleteCompound(Compound compound)
        {
            using var db = new MongoDBProvider(_chemMongoDbPath, "chemistry");
            db.Delete<Compound>("compounds", compound.Id); // Explicitly specify the type argument
        }
        #endregion
    }
}
