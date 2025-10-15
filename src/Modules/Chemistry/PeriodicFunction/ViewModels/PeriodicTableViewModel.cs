using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

using LaboratoryApp.src.Core.Caches.Chemistry;

using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Core.Models.Chemistry;

using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Services.Chemistry.PeriodicFunction;

namespace LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels
{
    public class PeriodicTableViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IChemistryDataCache _chemistryDataCache;
        private readonly IPeriodicService _periodicService;

        #region Properties
        public ObservableCollection<ElementInfoViewModel> ElementCells { get; set; }
        #endregion

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="chemistryService"></param>
        public PeriodicTableViewModel(IChemistryDataCache chemistryDataCache,
                                      IPeriodicService periodicService)
        {
            _chemistryDataCache = chemistryDataCache;
            _periodicService = periodicService;
        }

        /// <summary>
        /// Initialize the periodic table with all elements and their properties.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _chemistryDataCache.AllElements = _periodicService.LoadAllElements();
                ElementCells = new ObservableCollection<ElementInfoViewModel>(_chemistryDataCache.AllElements.Select(e => new ElementInfoViewModel(e)));
                OnPropertyChanged(nameof(ElementCells));
            }, cancellationToken);
        }
    }
}
