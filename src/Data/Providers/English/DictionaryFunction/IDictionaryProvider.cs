using LaboratoryApp.Domain.Models.English.DictionaryFunction;

namespace LaboratoryApp.src.Data.Providers.English.DictionaryFunction
{
    public interface IDictionaryProvider
    {
        Task<List<Word>> GetAllWordsAsync();
        Task<List<Pos>> GetAllPosAsync();
        Task<List<Example>> GetAllExamplesAsync();
        Task<List<Definition>> GetAllDefinitionsAsync();
    }
}
