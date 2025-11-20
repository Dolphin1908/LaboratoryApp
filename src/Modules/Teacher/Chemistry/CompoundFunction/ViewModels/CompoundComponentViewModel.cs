using LaboratoryApp.Domain.Models.Chemistry.Common;
using LaboratoryApp.src.Core.Caches.Chemistry;
using LaboratoryApp.src.Core.ViewModels;
using System.Collections.ObjectModel;

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
