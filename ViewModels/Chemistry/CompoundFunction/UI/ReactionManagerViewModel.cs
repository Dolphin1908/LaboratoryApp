using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.ViewModels.Chemistry.CompoundFunction.SubWin;
using LaboratoryApp.Views.Chemistry.CompoundFunction.SubWin;

namespace LaboratoryApp.ViewModels.Chemistry.CompoundFunction.UI
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
