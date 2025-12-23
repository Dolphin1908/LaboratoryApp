using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Content
{
    public class AnswerOptionProvider : IAnswerOptionProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<AnswerOption> _answerOptionCollection;

        public AnswerOptionProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);
            _answerOptionCollection = _mongoDb.GetCollection<AnswerOption>(CollectionName.AnswerOptions);
        }

        public async Task<List<AnswerOption>> GetAllAnswerOptionsAsync()
        {
            return await _answerOptionCollection.Find(FilterDefinition<AnswerOption>.Empty).ToListAsync();
        }

        public async Task<AnswerOption?> GetAnswerOptionByIdAsync(long id)
        {
            var filter = Builders<AnswerOption>.Filter.Eq(a => a.Id, id);
            return await _answerOptionCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<AnswerOption>> GetAnswerOptionsByIdsAsync(List<long> ids)
        {
            var filter = Builders<AnswerOption>.Filter.In(a => a.Id, ids);
            return await _answerOptionCollection.Find(filter).ToListAsync();
        }

        public async Task CreateNewAnswerOptionAsync(AnswerOption answerOption)
        {
            await _answerOptionCollection.InsertOneAsync(answerOption);
        }

        public async Task UpdateAnswerOptionAsync(AnswerOption answerOption)
        {
            var filter = Builders<AnswerOption>.Filter.Eq(a => a.Id, answerOption.Id);
            await _answerOptionCollection.ReplaceOneAsync(filter, answerOption);
        }

        public async Task DeleteAnswerOptionAsync(long id)
        {
            var filter = Builders<AnswerOption>.Filter.Eq(a => a.Id, id);
            await _answerOptionCollection.DeleteOneAsync(filter);
        }
    }
}