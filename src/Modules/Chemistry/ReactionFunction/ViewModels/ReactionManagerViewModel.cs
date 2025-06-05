using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.ReactionFunction.Views;

namespace LaboratoryApp.src.Modules.Chemistry.ReactionFunction.ViewModels
{
    public class ReactionManagerViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        #region Commands
        public ICommand AddReactionCommand { get; set; } // Command to open the add reaction window
        #endregion

        public ReactionManagerViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // Initialize commands

            AddReactionCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open the add reaction window
                var window = _serviceProvider.GetRequiredService<AddReactionWindow>();
                window.Show();
            });
        }
    }
}
