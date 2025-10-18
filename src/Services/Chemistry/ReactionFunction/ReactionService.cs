using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Data.Providers.Chemistry.ReactionFunction;
using LaboratoryApp.src.Services.Helper.Counter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.Chemistry.ReactionFunction
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionProvider _reactionProvider;
        private readonly ICounterService _counterService;
        private readonly IChemistryDataCache _chemistryDataCache;

        public ReactionService(IReactionProvider reactionProvider,
                               ICounterService counterService,
                               IChemistryDataCache chemistryDataCache)
        {
            _reactionProvider = reactionProvider;
            _counterService = counterService;
            _chemistryDataCache = chemistryDataCache;
        }

        /// <summary>
        /// Lấy danh sách gợi ý phản ứng dựa trên chất tham gia và sản phẩm
        /// </summary>
        /// <param name="Reactants"></param>
        /// <param name="Products"></param>
        /// <returns></returns>
        public IEnumerable<Reaction> GetReactionSuggestions(string Reactants, string Products)
        {
            // Split the reactants and products strings into lists
            List<string> reactants = string.IsNullOrWhiteSpace(Reactants) ? new List<string>() : Reactants.Split(' ').ToList();
            List<string> products = string.IsNullOrWhiteSpace(Products) ? new List<string>() : Products.Split(' ').ToList();

            // Search for reactions that match the given reactants and products
            return _chemistryDataCache.AllReactions.AsParallel()
                                                  .Where(r =>
                                                  {
                                                      var rReactants = r.Reactants.Select(x => _chemistryDataCache.AllCompounds.FirstOrDefault(c => c.Id == x.CompoundId)?.Formula ?? _chemistryDataCache.AllElements.FirstOrDefault(c => c.Id == x.ElementId)?.Formula ?? string.Empty)
                                                                                  .Where(x => !string.IsNullOrWhiteSpace(x))
                                                                                  .ToList();

                                                      var rProducts = r.Products.Select(x => _chemistryDataCache.AllCompounds.FirstOrDefault(c => c.Id == x.CompoundId)?.Formula ?? _chemistryDataCache.AllElements.FirstOrDefault(c => c.Id == x.ElementId)?.Formula ?? string.Empty)
                                                                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                                                                .ToList();

                                                      bool reactantsMatch = !reactants.Any() || reactants.All(x => rReactants.Contains(x, StringComparer.OrdinalIgnoreCase));
                                                      bool productsMatch = !products.Any() || products.All(x => rProducts.Contains(x, StringComparer.OrdinalIgnoreCase));

                                                      return reactantsMatch && productsMatch;
                                                  }).ToList();
        }

        public IEnumerable<object> GetElementCompoundSuggestions(string SearchText, SubstanceKind Kind)
        {
            if (Kind == SubstanceKind.Element)
            {
                // Tìm kiếm theo tên hoặc công thức
                return _chemistryDataCache.AllElements.Where(e => e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || 
                                                                 e.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                                                     .ToList();
            }
            else
            {
                // Tìm kiếm theo công thức
                return _chemistryDataCache.AllCompounds.Where(c => c.Formula.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                                                      .ToList();
            }
        }

        /// <summary>
        /// Lưu phản ứng vào cơ sở dữ liệu
        /// </summary>
        /// <param name="reaction"></param>
        public void SaveReaction(Reaction reaction)
        {
            reaction.Id = _counterService.GetNextId(CollectionName.Reactions);

            reaction.OwnerId = AuthenticationCache.CurrentUser?.Id ?? 0;

            if(string.IsNullOrWhiteSpace(reaction.Author))
            {
                reaction.Author = "Unknown";
            }    

            _reactionProvider.AddReaction(reaction);
            _chemistryDataCache.AllReactions.Add(reaction);
        }
    }
}
