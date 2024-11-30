using LaboratoryApp.Database.Provider;
using LaboratoryApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.ViewModels.UC
{
    public class ElementCellViewModel : BaseViewModel
    {
        #region commands
        public ICommand CellClickedCommand { get; set; }
        #endregion

        private ObservableCollection<ElementModel> _elements;
        public ObservableCollection<ElementModel> Elements
        {
            get { return _elements; }
            set
            {
                _elements = value;
                OnPropertyChanged();
            }
        }

        public ElementCellViewModel()
        {
            var elements = SQLiteDataProvider.Instance.ExecuteQuery<ElementModel>("SELECT * FROM Elements");
            Elements = new ObservableCollection<ElementModel>(elements);
            MessageBox.Show(Elements[1].color);
        }

    }
}
