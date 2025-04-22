using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.Models.Chemistry;
using LaboratoryApp.Views.Chemistry.PeriodicTable.SubWin;

namespace LaboratoryApp.ViewModels.Chemistry.PeriodicTable.SubWin
{
    public class ElementInfoViewModel : BaseViewModel
    {
        #region commands
        public ICommand ElementCellClickedCommand { get; set; }
        public ICommand CloseElementInfoCommand { get; set; }
        #endregion

        private ElementModel _element;
        public ElementModel Element
        {
            get { return _element; }
            set
            {
                _element = value;
                OnPropertyChanged();
            }
        }

        // Get element row and column for periodic table
        public int Row => Element.row - 1;
        public int Column => Element.column - 1;

        public ElementInfoViewModel(ElementModel element)
        {
            Element = element;

            // Handle element cell clicked
            ElementCellClickedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new ElementInfo
                {
                    DataContext = this
                };
                window.Title = Element.Name;
                window.ShowDialog();
            });

            // Handle close window
            CloseElementInfoCommand = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
        }
    }
}
