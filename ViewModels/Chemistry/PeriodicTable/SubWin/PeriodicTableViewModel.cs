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
        private ElementModel LoadColorCategory(ElementModel element)
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
                Phase = "Chất rắn",
                Color = "",
                Category = "Họ Lanthan",
                DiscoveryYear = "1838",
                Row = 6,
                Column = 3,
            });
            elements.Add(new ElementModel
            {
                AtomicNumber = "89 - 103",
                Name = "Actinides",
                Symbol = "Ac - Lr",
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

            return elements;
        }
    }
}
