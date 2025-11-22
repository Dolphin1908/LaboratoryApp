using LaboratoryApp.Domain.Interfaces.Providers.English;
using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.English
{
    public class DiaryProvider : IDiaryProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<DiaryContent> _diaryCollection;

        public DiaryProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.EnglishMongoDB);
            _diaryCollection = _mongoDb.GetCollection<DiaryContent>(CollectionName.Diaries);
        }

        /// <summary>
        /// Add a new diary entry to the MongoDB database.
        /// </summary>
        /// <param name="diary"></param>
        public async Task AddDiaryAsync(DiaryContent diary)
        {
            await _diaryCollection.InsertOneAsync(diary);
        }

        /// <summary>
        /// Get all diary entries from the MongoDB database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<DiaryContent>> GetAllDiariesAsync()
        {
            var diaries = await _diaryCollection.Find(FilterDefinition<DiaryContent>.Empty).ToListAsync();
            return diaries;
        }

        /// <summary>
        /// Update an existing diary entry.
        /// </summary>
        /// <param name="diary"></param>
        public async Task UpdateDiaryAsync(DiaryContent diary)
        {
            await _diaryCollection.ReplaceOneAsync<DiaryContent>(d => d.Id == diary.Id, diary);
        }

        /// <summary>
        /// Delete a diary entry by its ID.
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteDiaryAsync(long id)
        {
            await _diaryCollection.DeleteOneAsync(d => d.Id == id);
        }
    }
}