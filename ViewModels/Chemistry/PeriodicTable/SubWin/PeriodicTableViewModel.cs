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

using LaboratoryApp.Database.Provider;
using LaboratoryApp.Models.Chemistry;
using LaboratoryApp.Services;
using LaboratoryApp.ViewModels.UC;

namespace LaboratoryApp.ViewModels.Chemistry.PeriodicTable.SubWin
{
    public class PeriodicTableViewModel : BaseViewModel
    {
        #region Commands

        #endregion

        private ChemistryService _chemistryService;
        private List<ElementModel> _elements;
        public ObservableCollection<ElementInfoViewModel> ElementCells { get; set; }


        public PeriodicTableViewModel()
        {
            _chemistryService = new ChemistryService();
            _elements = LoadElement();
            ElementCells = new ObservableCollection<ElementInfoViewModel>(_elements.Select(e => new ElementInfoViewModel(e)));
        }

        /// <summary>
        /// Load period block for each element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private ElementModel LoadPeriodBlock(ElementModel element)
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
                    element.row = 5;
                }
                else if (atomicNumber >= 57 && atomicNumber <= 71) // Lanthanide
                {
                    element.row = 9;
                }
                else if (atomicNumber >= 89 && atomicNumber <= 103) // Actinide
                {
                    element.row = 10;
                }
                else
                {
                    element.row = row;
                }

                // Expect elements with column
                if (atomicNumber >= 72 && atomicNumber <= 86 || atomicNumber >= 104 && atomicNumber <= 118) // Lanthanide and Actinide
                {
                    element.column = column - 14;
                }
                else
                {
                    element.column = column;
                }
            }

            return element;
        }

        /// <summary>
        /// Load color category for each element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private ElementModel LoadColorCategory(ElementModel element)
        {
            if (element.Category == "Nonmetal")
                element.Color = "ab91ff";
            else if (element.Category == "Noble gas")
                element.Color = "91dfff";
            else if (element.Category == "Alkali metal")
                element.Color = "ffc591";
            else if (element.Category == "Alkaline earth metal")
                element.Color = "ffdf91";
            else if (element.Category == "Metalloid")
                element.Color = "92ff9f";
            else if (element.Category == "Halogen")
                element.Color = "f991ff";
            else if (element.Category == "Post-transition metal")
                element.Color = "fff991";
            else if (element.Category == "Transition metal")
                element.Color = "ecff91";
            else if (element.Category == "Lanthanide")
                element.Color = "d2ff91";
            else if (element.Category == "Actinide")
                element.Color = "b8ff91";
            else if (element.Category == "Unknown")
                element.Color = "c8c8c8";

            return element;
        }

        /// <summary>
        /// Load elements from database
        /// </summary>
        /// <returns></returns>
        private List<ElementModel> LoadElement()
        {
            var elements = _chemistryService.GetAllElements();

            // Add Lanthanide group and Actinide group
            elements.Add(new ElementModel
            {
                AtomicNumber = "57 - 71",
                Name = "Lanthanides",
                Symbol = "La - Lu",
                Phase = "Solid",
                Color = "",
                Category = "Lanthanide",
                DiscoveryYear = "1838",
                row = 6,
                column = 3,
            });
            elements.Add(new ElementModel
            {
                AtomicNumber = "89 - 103",
                Name = "Actinides",
                Symbol = "Ac - Lr",
                Phase = "Solid",
                Color = "",
                Category = "Actinide",
                DiscoveryYear = "1940",
                row = 7,
                column = 3,
            });

            foreach (var element in elements)
            {
                LoadPeriodBlock(element);
                LoadColorCategory(element);
            }

            return elements;
        }
    }
}
