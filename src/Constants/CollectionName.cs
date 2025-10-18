using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Constants
{
    public static class CollectionName
    {
        #region Authentication
        public const string Users = "users";
        public const string RefreshTokens = "refresh_tokens";
        #endregion

        #region Authorization
        public const string ExerciseSetAccess = "exercise_set_access";
        #endregion

        #region Chemistry
        public const string Compounds = "compounds";
        public const string Elements = "elements";
        public const string Reactions = "reactions";
        #endregion

        #region English
        public const string Words = "Words";
        public const string Pos = "Pos";
        public const string Definitions = "Definitions";
        public const string Examples = "Examples";
        public const string Diaries = "diaries";
        #endregion

        #region Assignment
        public const string ExerciseSets = "exercise_sets";
        public const string Exercises = "exercises";
        #endregion

        #region Helper
        public const string Counters = "counters";
        #endregion
    }
}
