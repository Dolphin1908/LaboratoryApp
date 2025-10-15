using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.English;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction;
using LaboratoryApp.src.Core.Models.English.DictionaryFunction.DTOs;

namespace LaboratoryApp.src.Services.English.DictionaryFunction
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IEnglishDataCache _englishDataCache;

        private readonly Dictionary<long, List<Pos>> _posByWordId;
        private readonly Dictionary<long, List<Definition>> _definitionsByPosId;
        private readonly Dictionary<long, List<Example>> _examplesByDefId;
        private readonly List<Word> _allWords;

        /// <summary>
        /// Constructor initializes the in-memory data structures for fast access.
        /// </summary>
        public DictionaryService(IEnglishDataCache englishDataCache)
        {
            _englishDataCache = englishDataCache;

            _allWords = _englishDataCache.AllWords;

            _posByWordId = _allWords.ToDictionary(w => w.Id, w => new List<Pos>());
            var allPos = _englishDataCache.AllPos;
            foreach (var pos in allPos)
            {
                if (_posByWordId.ContainsKey(pos.WordId))
                {
                    _posByWordId[pos.WordId].Add(pos);
                }
            }

            _definitionsByPosId = allPos.ToDictionary(p => p.Id, p => new List<Definition>());
            var allDefinitions = _englishDataCache.AllDefinitions;
            foreach (var def in allDefinitions)
            {
                if (_definitionsByPosId.ContainsKey(def.PosId))
                {
                    _definitionsByPosId[def.PosId].Add(def);
                }    
            }    

            _examplesByDefId = allDefinitions.ToDictionary(d => d.Id, d => new List<Example>());
            var allExamples = _englishDataCache.AllExamples;
            foreach (var ex in allExamples)
            {
                if (_examplesByDefId.ContainsKey(ex.DefId))
                {
                    _examplesByDefId[ex.DefId].Add(ex);
                }
            }
        }

        /// <summary>
        /// Builds the WordResultDTO object for the selected word.
        /// </summary>
        /// <param name="word">Selected word</param>
        /// <returns></returns>
        public WordResultDTO? BuildWordResultDTO(Word word)
        {
            if (word == null) return null;

            var posDTOs = _posByWordId.GetValueOrDefault(word.Id, new List<Pos>())
                                      .Select(pos => new PosDTO
                                      {
                                          Id = pos.Id,
                                          WordId = pos.WordId,
                                          Pos = pos.Content,
                                          Definitions = _definitionsByPosId.GetValueOrDefault(pos.Id, new List<Definition>())
                                                                           .Select(def => new DefinitionDTO
                                                                           {
                                                                               Id = def.Id,
                                                                               PosId = def.PosId,
                                                                               Definition = def.Content,
                                                                               Examples = _examplesByDefId.GetValueOrDefault(def.Id, new List<Example>())
                                                                                                          .Select(ex =>
                                                                                                          {
                                                                                                              var parts = ex.Content.Split(new[] { '+' }, 2);
                                                                                                              return new ExampleDTO
                                                                                                              {
                                                                                                                  Id = ex.Id,
                                                                                                                  DefId = ex.DefId,
                                                                                                                  Example = parts[0].Trim(),
                                                                                                                  Translation = parts.Length > 1 ? parts[1].Trim() : string.Empty
                                                                                                              };
                                                                                                          }).ToList()
                                                                           }).ToList()
                                      }).ToList();

            return new WordResultDTO
            {
                Id = word.Id,
                Word = word.Content,
                Pronunciation = word.Pronunciation,
                Pos = posDTOs
            };
        }

        /// <summary>
        /// Gets search suggestions based on the input text.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IEnumerable<DictionarySearchResultDTO> GetSuggestions(string searchText, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return Enumerable.Empty<DictionarySearchResultDTO>();
            }

            return _allWords.Where(w => w.Content.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                            .Take(limit)
                            .Select(word =>
                            {
                                var firstPos = _posByWordId.GetValueOrDefault(word.Id)?.FirstOrDefault();
                                var firstDef = firstPos != null ? _definitionsByPosId.GetValueOrDefault(firstPos.Id)?.FirstOrDefault() : null;

                                return new DictionarySearchResultDTO
                                {
                                    WordId = word.Id,
                                    Word = word.Content,
                                    Pos = firstPos?.Content ?? string.Empty,
                                    Meaning = firstDef?.Content ?? string.Empty
                                };
                            });
        }

        /// <summary>
        /// Gets a word by its ID.
        /// </summary>
        /// <param name="wordId"></param>
        /// <returns></returns>
        public Word? GetWordById(long wordId)
        {
            return _allWords.FirstOrDefault(w => w.Id == wordId);
        }
    }
}
