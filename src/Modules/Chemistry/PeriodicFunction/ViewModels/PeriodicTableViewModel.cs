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

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Services.Chemistry;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels
{
    public class PeriodicTableViewModel : BaseViewModel, IAsyncInitializable
    {
        #region Commands

        #endregion

        private List<Element> _elements;
        private readonly IChemistryService _chemistryService;

        public ObservableCollection<ElementInfoViewModel> ElementCells { get; set; }

        public PeriodicTableViewModel(IChemistryService chemistryService)
        {
            _chemistryService = chemistryService;
        }

        /// <summary>
        /// Load period block for each element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private Element LoadPeriodBlock(Element element)
        {
            // Check if the element is a lanthanide or actinide
            if (element.AtomicNumber.IndexOf('-') == -1)
            {
                string input = element.ElectronConfiguration;
                string pattern = @"(\d+)([sfdp])(\d+)";

                MatchCollection matches = Regex.Matches(input, pattern);
                int column = 0;
                int row = 0;
                foreach (Match match in matches)
                {
                    string principalQuantumNumber = match.Groups[1].Value; // Principal quantum number
                    string sublevel = match.Groups[2].Value; // Sublevel (s, p, d, f)
                    string electrons = match.Groups[3].Value; // Number of electrons in the sublevel
                    row = Math.Max(int.Parse(principalQuantumNumber), row); // Get the maximum principal quantum number
                    column += int.Parse(electrons); // Add the number of electrons in the sublevel
                }

                if (row == 1 && column != 1) column += 16;
                else if (row <= 3 && column >= 3) column += 10;

                var atomicNumber = int.Parse(element.AtomicNumber);

                // Expect elements with row
                if (atomicNumber == 46)
                {
                    element.Row = 5;
                }
                else if (atomicNumber >= 57 && atomicNumber <= 71) // Lanthanide
                {
                    element.Row = 9;
                }
                else if (atomicNumber >= 89 && atomicNumber <= 103) // Actinide
                {
                    element.Row = 10;
                }
                else
                {
                    element.Row = row;
                }

                // Expect elements with column
                if (atomicNumber >= 72 && atomicNumber <= 86 || atomicNumber >= 104 && atomicNumber <= 118) // Lanthanide and Actinide
                {
                    element.Column = column - 14;
                }
                else
                {
                    element.Column = column;
                }
            }

            return element;
        }

        /// <summary>
        /// Load color category for each element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private Element LoadColorCategory(Element element)
        {
            if (element.Category == "Phi kim")
                element.Color = "ab91ff";
            else if (element.Category == "Khí hiếm")
                element.Color = "91dfff";
            else if (element.Category == "Kim loại kiềm")
                element.Color = "ffc591";
            else if (element.Category == "Kim loại kiềm thổ")
                element.Color = "ffdf91";
            else if (element.Category == "Á kim")
                element.Color = "92ff9f";
            else if (element.Category == "Halogen")
                element.Color = "f991ff";
            else if (element.Category == "Kim loại sau chuyển tiếp")
                element.Color = "fff991";
            else if (element.Category == "Kim loại chuyển tiếp")
                element.Color = "ecff91";
            else if (element.Category == "Họ Lanthan")
                element.Color = "d2ff91";
            else if (element.Category == "Họ Actini")
                element.Color = "b8ff91";
            else if (element.Category == "Unknown")
                element.Color = "c8c8c8";

            return element;
        }

        /// <summary>
        /// Initialize the periodic table with all elements and their properties.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var elements = await Task.Run(() => _chemistryService.GetAllElements(), cancellationToken);

            elements.Add(new Element
            {
                AtomicNumber = "57 - 71",
                Name = "Lanthanides",
                Formula = "La - Lu",
                Phase = "Chất rắn",
                Color = "",
                Category = "Họ Lanthan",
                DiscoveryYear = "1838",
                Row = 6,
                Column = 3,
            });

            elements.Add(new Element
            {
                AtomicNumber = "89 - 103",
                Name = "Actinides",
                Formula = "Ac - Lr",
                Phase = "Chất rắn",
                Color = "",
                Category = "Họ Actini",
                DiscoveryYear = "1940",
                Row = 7,
                Column = 3,
            });

            foreach (var element in elements)
            {
                LoadPeriodBlock(element);
                LoadColorCategory(element);
            }

            // Update the elements in the view model
            Application.Current.Dispatcher.Invoke(() =>
            {
                _elements = elements;
                ElementCells = new ObservableCollection<ElementInfoViewModel>(_elements.Select(e => new ElementInfoViewModel(e)));
                OnPropertyChanged(nameof(ElementCells));
            });

        }
    }
}
