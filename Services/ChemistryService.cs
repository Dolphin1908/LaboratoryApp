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
        private readonly string _chemDbPath = AppConfigHelper.GetKey("ChemistryDbPath");

        public List<ElementModel> GetAllElements()
        {
            using var db = new SQLiteDataProvider(_chemDbPath);
            return db.ExecuteQuery<ElementModel>("SELECT * FROM Elements");
        }
    }
}
