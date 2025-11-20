using LaboratoryApp.Domain.DTOs.English.DictionaryFunction;
using LaboratoryApp.Domain.Models.English.DictionaryFunction;

namespace LaboratoryApp.src.Services.English.DictionaryFunction
{
    public interface IDictionaryService
    {
        WordResultDTO BuildWordResultDTO(Word word);
        IEnumerable<DictionarySearchResultDTO> GetSuggestions(string searchText, int limit = 10);
        Word? GetWordById(long wordId);
    }
}
