using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Data.Providers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.English.DiaryFunction
{
    public class DiaryProvider : IDiaryProvider
    {
        private readonly IMongoDBProvider _mongoDb;

        public DiaryProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.EnglishMongoDB);
        }

        /// <summary>
        /// Add a new diary entry to the MongoDB database.
        /// </summary>
        /// <param name="diary"></param>
        public void AddDiary(DiaryContent diary)
        {
            _mongoDb.Insert(CollectionName.Diaries, diary);
        }

        /// <summary>
        /// Get all diary entries from the MongoDB database.
        /// </summary>
        /// <returns></returns>
        public List<DiaryContent> GetAllDiaries()
        {
            return _mongoDb.GetAll<DiaryContent>(CollectionName.Diaries);
        }

        /// <summary>
        /// Update an existing diary entry.
        /// </summary>
        /// <param name="diary"></param>
        public void UpdateDiary(DiaryContent diary)
        {
            _mongoDb.Update(CollectionName.Diaries, diary.Id, diary);
        }

        /// <summary>
        /// Delete a diary entry by its ID.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDiary(long id)
        {
            _mongoDb.Delete<DiaryContent>(CollectionName.Diaries, id);
        }
    }
}