using LaboratoryApp.Domain.Models.Chemistry.ReactionFunction;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Modules.Tools.Chemistry.DictionaryFunction.ReactionFunction.ViewModels
{
    public class ReactionSelectionResultViewModel : BaseViewModel
    {
        private readonly IChemistryDataCache _cache;
        private readonly Reaction _selectedReaction;

        #region Properties
        public Reaction SelectedReaction
        {
            get => _selectedReaction;
        }
        #endregion

        public ReactionSelectionResultViewModel(IChemistryDataCache cache, Reaction selectedReaction)
        {
            _cache = cache;
            _selectedReaction = selectedReaction;
        }
    }
}
