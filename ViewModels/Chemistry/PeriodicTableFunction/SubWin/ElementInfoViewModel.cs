using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.Models.Chemistry;
using LaboratoryApp.Views.Chemistry.PeriodicTableFunction.SubWin;

namespace LaboratoryApp.ViewModels.Chemistry.PeriodicTableFunction.SubWin
{
    public class ElementInfoViewModel : BaseViewModel
    {
        #region commands
        public ICommand ElementCellClickedCommand { get; set; }
        public ICommand CloseElementInfoCommand { get; set; }
        #endregion

        private Element _element;
        public Element Element
        {
            get { return _element; }
            set
            {
                _element = value;
                OnPropertyChanged();
            }
        }

        // Get element row and column for periodic table
        public int Row => Element.Row - 1;
        public int Column => Element.Column - 1;

        public ElementInfoViewModel(Element element)
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
