using LaboratoryApp.Database;
using LaboratoryApp.Models;
using LaboratoryApp.ViewModels.UI;
using LaboratoryApp.Views.UC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LaboratoryApp.ViewModels.UC
{
    public class ControlBarViewModel : BaseViewModel
    {
        #region commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        public ICommand ToggleClickCommand { get; set; }
        #endregion

        private MainWindowViewModel _mainWindowVM;

        public ControlBarViewModel(MainWindowViewModel mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;

            ToggleClickCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _mainWindowVM.IsNavigationVisible = !_mainWindowVM.IsNavigationVisible;
            });
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = Window.GetWindow(p);
                if (w != null)
                {
                    w.Close();
                }
            });
            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = Window.GetWindow(p);
                if (w != null)
                {
                    if (w.WindowState != WindowState.Maximized)
                        w.WindowState = WindowState.Maximized;
                    else 
                        w.WindowState = WindowState.Normal;
                }
            });
            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = Window.GetWindow(p);
                if (w != null)
                {
                    if (w.WindowState != WindowState.Minimized)
                        w.WindowState = WindowState.Minimized;
                }
            
            });
            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = Window.GetWindow(p);
                if (w != null)
                {
                    w.DragMove();
                }
            });
        }
    }
}
