using LaboratoryApp.Domain.Models.Chemistry.Common;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.Chemistry.PeriodicFunction.ViewModels
{
    public class ElementInfoViewModel : BaseViewModel
    {
        #region Commands
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

        public ElementInfoViewModel()
        {
            // Default constructor
        }

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
