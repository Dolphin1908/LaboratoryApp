using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using LaboratoryApp.Database.Provider;
using LaboratoryApp.ViewModels.UC;

namespace LaboratoryApp.ViewModels.UI
{
    public class MainWindowViewModel : BaseViewModel
    {
        private bool _isNavigationVisible;
        public bool IsNavigationVisible
        {
            get => _isNavigationVisible;
            set
            {
                _isNavigationVisible = value;
                OnPropertyChanged(); // Notify the UI about the change
            }
        }
        public ControlBarViewModel ControlBarVM { get; set; }

        public MainWindowViewModel()
        {
            ControlBarVM = new ControlBarViewModel(this);
        }
    }
}
