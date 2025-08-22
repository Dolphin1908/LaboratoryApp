using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionSelectionResultViewModel : BaseViewModel
    {
        private readonly Reaction _selectedReaction;

        #region Properties
        public Reaction SelectedReaction
        {
            get => _selectedReaction;
        }
        #endregion

        public ReactionSelectionResultViewModel(Reaction selectedReaction)
        {
            _selectedReaction = selectedReaction ?? throw new ArgumentNullException(nameof(selectedReaction), "Selected reaction cannot be null.");
        }
    }
}
