using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.English.DictionaryFunction
{
    public interface IDictionaryService
    {
        WordResultDTO BuildWordResultDTO(Word word);
        IEnumerable<DictionarySearchResultDTO> GetSuggestions(string searchText, int limit = 10);
        Word? GetWordById(long wordId);
    }
}
