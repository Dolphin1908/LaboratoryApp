using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;

namespace LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionManagerViewModel : BaseViewModel
    {
        #region Commands
        public ICommand AddReactionCommand { get; set; } // Command to open the add reaction window
        #endregion

        public ReactionManagerViewModel()
        {
            // Initialize commands

            AddReactionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open the add reaction window
                var addReactionWindow = new AddReactionWindow
                {
                    DataContext = new ReactionViewModel()
                };
                addReactionWindow.Show();
            });
        }
    }
}
