using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Data.Providers.Chemistry.CompoundFunction;
using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;
using LaboratoryApp.src.Services.Helper.Counter;

namespace LaboratoryApp.src.Services.Chemistry.CompoundFunction
{
    public class CompoundService : ICompoundService
    {
        private readonly ICompoundProvider _compoundProvider;
        private readonly ICounterService _counterService;
        private readonly IChemistryDataCache _chemistryDataCache;

        public CompoundService(ICompoundProvider compoundProvider,
                               ICounterService counterService,
                               IChemistryDataCache chemistryDataCache)
        {
            _compoundProvider = compoundProvider;
            _counterService = counterService;
            _chemistryDataCache = chemistryDataCache;
        }

        /// <summary>
        /// Lấy danh sách gợi ý hợp chất dựa trên văn bản tìm kiếm
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IEnumerable<Compound> GetSuggestions(string searchText, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return Enumerable.Empty<Compound>();
            }

            return _chemistryDataCache.AllCompounds.Where(c => c.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                                              c.Formula.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                                                  .Take(limit);
        }

        /// <summary>
        /// Save compound to database
        /// </summary>
        /// <param name="compound"></param>
        /// <param name="composition"></param>
        /// <param name="notes"></param>
        /// <param name="physicalProperties"></param>
        /// <param name="chemicalProperties"></param>
        /// <param name="compoundTypeOptions"></param>
        /// <param name="phaseOptions"></param>
        public void SaveCompound(Compound compound)
        {
            compound.Id = _counterService.GetNextId(CollectionName.Compounds);

            compound.OwnerId = AuthenticationCache.CurrentUser?.Id ?? 0;

            if(string.IsNullOrWhiteSpace(compound.Author))
            {
                compound.Author = "Unknown";
            }

            _compoundProvider.AddCompound(compound);
            _chemistryDataCache.AllCompounds.Add(compound);
        }
    }
}
