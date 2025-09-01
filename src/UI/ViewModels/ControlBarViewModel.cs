using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.UI.ViewModels
{
    public class ControlBarViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        public ICommand ToggleClickCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }
        #endregion

        private MainWindowViewModel _mainWindowVM;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainWindowVM"></param>
        public ControlBarViewModel(MainWindowViewModel mainWindowVM,
                                   INavigationService navigationService)
        {
            _mainWindowVM = mainWindowVM;
            _navigationService = navigationService;

            // Handle the click event of the toggle button to show/hide the navigation bar
            ToggleClickCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _mainWindowVM.IsNavigationVisible = !_mainWindowVM.IsNavigationVisible;
            });

            // Handle the close window command
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = Window.GetWindow(p);
                if (w != null)
                {
                    w.Close();
                }
            });

            // Handle the maximize window command
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

            // Handle the minimize window command
            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = Window.GetWindow(p);
                if (w != null)
                {
                    if (w.WindowState != WindowState.Minimized)
                        w.WindowState = WindowState.Minimized;
                }
            
            });

            // Handle the mouse move window command
            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = Window.GetWindow(p);
                if (w != null)
                {
                    w.DragMove();
                }
            });

            NavigateBackCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                _navigationService.NavigateBack();
            });
        }
    }
}
