using LaboratoryApp.ViewModels.Chemistry.CompoundFunction.SubWin;
using LaboratoryApp.Views.Chemistry.CompoundFunction.SubWin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.ViewModels.Chemistry.CompoundFunction.UI
{
    public class CompoundManagerViewModel : BaseViewModel
    {
        private string _searchText;

        #region Commands
        public ICommand AddCompoundCommand { get; set; }
        #endregion

        #region Properties
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public CompoundManagerViewModel()
        {
            AddCompoundCommand = new RelayCommand<object>(p => true, p =>
            {
                var addCompoundWindow = new AddCompoundWindow
                {
                    DataContext = new CompoundViewModel()
                };
                addCompoundWindow.ShowDialog();
            });
        }
    }
}
