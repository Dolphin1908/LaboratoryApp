using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;

namespace LaboratoryApp.src.Services.English
{
    public interface IEnglishService
    {
        #region ExerciseMongoDB

        #endregion

        #region DiaryMongoDB
        public void AddDiary(DiaryContent diary);
        public List<DiaryContent> GetAllDiaries();
        public void UpdateDiary(DiaryContent diary);
        public void DeleteDiary(long id);
        #endregion

        #region DictionarySQLite
        public List<Word> GetAllWords();
        public List<Pos> GetAllPos();
        public List<Example> GetAllExamples();
        public List<Definition> GetAllDefinitions();
        #endregion
    }
}
