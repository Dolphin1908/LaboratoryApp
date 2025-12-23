using LaboratoryApp.Domain.Interfaces.Providers.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Data.Providers.Common;
using MongoDB.Driver;

namespace LaboratoryApp.src.Data.Providers.Content
{
    public class QuestionProvider : IQuestionProvider
    {
        private readonly IMongoDBProvider _mongoDb;
        private readonly IMongoCollection<Question> _questionCollection;

        public QuestionProvider(IEnumerable<IMongoDBProvider> mongoDb)
        {
            _mongoDb = mongoDb.First(d => d.DatabaseName == DatabaseName.AssignmentMongoDB);
            _questionCollection = _mongoDb.GetCollection<Question>(CollectionName.Questions);
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            var questions = await _questionCollection.Find(FilterDefinition<Question>.Empty).ToListAsync();
            return questions;
        }

        public async Task<Question?> GetQuestionByIdAsync(long id)
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, id);
            return await _questionCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Question>> GetQuestionsByIdsAsync(List<long> ids)
        {
            var filter = Builders<Question>.Filter.In(q => q.Id, ids);
            return await _questionCollection.Find(filter).ToListAsync();
        }

        public async Task CreateNewQuestionAsync(Question question)
        {
            await _questionCollection.InsertOneAsync(question);
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, question.Id);
            await _questionCollection.ReplaceOneAsync(filter, question);
        }

        public async Task DeleteQuestionAsync(long id)
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, id);
            await _questionCollection.DeleteOneAsync(filter);
        }
    }
}
