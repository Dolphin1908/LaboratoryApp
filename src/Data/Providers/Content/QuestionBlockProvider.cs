using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Content
{
    public class QuestionBlockProvider : IQuestionBlockProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<QuestionBlock> _questionBlockCollection;

        public QuestionBlockProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);
            _questionBlockCollection = _mongoDb.GetCollection<QuestionBlock>(CollectionName.QuestionBlocks);
        }

        public async Task<List<QuestionBlock>> GetAllQuestionBlocksAsync()
        {
            return await _questionBlockCollection.Find(FilterDefinition<QuestionBlock>.Empty).ToListAsync();
        }

        public async Task<QuestionBlock?> GetQuestionBlockByIdAsync(long id)
        {
            var filter = Builders<QuestionBlock>.Filter.Eq(q => q.Id, id);
            return await _questionBlockCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<QuestionBlock>> GetQuestionBlocksByIdsAsync(List<long> ids)
        {
            var filter = Builders<QuestionBlock>.Filter.In(q => q.Id, ids);
            return await _questionBlockCollection.Find(filter).ToListAsync();
        }

        public async Task CreateNewQuestionBlockAsync(QuestionBlock questionBlock)
        {
            await _questionBlockCollection.InsertOneAsync(questionBlock);
        }

        public async Task UpdateQuestionBlockAsync(QuestionBlock questionBlock)
        {
            var filter = Builders<QuestionBlock>.Filter.Eq(q => q.Id, questionBlock.Id);
            await _questionBlockCollection.ReplaceOneAsync(filter, questionBlock);
        }

        public async Task DeleteQuestionBlockAsync(long id)
        {
            var filter = Builders<QuestionBlock>.Filter.Eq(q => q.Id, id);
            await _questionBlockCollection.DeleteOneAsync(filter);
        }
    }
}
