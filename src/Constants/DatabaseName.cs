using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Constants
{
    public static class DatabaseName
    {
        public const string AuthenticationMongoDB = "authentication";
        public const string AuthorizationMongoDB = "authorization";
        public const string ChemistryMongoDB = "chemistry";
        public const string EnglishMongoDB = "english";
        public const string AssignmentMongoDB = "assignment";
        public const string HelperMongoDB = "helper";

        public static string ChemistrySQLite = "chem";
        public static string EnglishSQLite = "dictionary";
    }
}
