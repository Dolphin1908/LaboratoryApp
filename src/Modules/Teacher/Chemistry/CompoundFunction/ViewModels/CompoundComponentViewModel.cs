using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels
{
    public class CompoundComponentViewModel : BaseViewModel
    {
        private readonly IChemistryDataCache _chemistryDataCache;

        private string _quantity;
        private Element _selectedElement;

        #region Properties
        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public Element SelectedElement
        {
            get => _selectedElement;
            set
            {
                _selectedElement = value;
                OnPropertyChanged();
                if (value != null)
                    Quantity = "1";
            }
        }
        #endregion

        private ObservableCollection<Element> _allElements;

        public CompoundComponentViewModel(IChemistryDataCache chemistryDataCache)
        {
            _chemistryDataCache = chemistryDataCache;

            _allElements = new ObservableCollection<Element>(_chemistryDataCache.AllElements);

            SelectedElement = _allElements.First();
        }
    }
}
