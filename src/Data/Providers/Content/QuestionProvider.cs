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
    }
}
