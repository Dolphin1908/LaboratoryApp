using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels
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
