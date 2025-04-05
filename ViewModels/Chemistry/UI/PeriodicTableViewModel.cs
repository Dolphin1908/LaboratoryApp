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
using LaboratoryApp.ViewModels.Chemistry.SubWin;
using LaboratoryApp.ViewModels.UC;

namespace LaboratoryApp.ViewModels.Chemistry.UI
{
    public class PeriodicTableViewModel : BaseViewModel
    {
        #region Commands

        #endregion

        private List<ElementModel> _elements;
        public ObservableCollection<ElementInfoViewModel> ElementCells { get; set; }


        public PeriodicTableViewModel()
        {
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
            if (element.atomic_number.IndexOf('-') == -1)
            {
                string input = element.electron_configuration;
                string pattern = @"(\d+)([sfdp])(\d+)";

                MatchCollection matches = Regex.Matches(input, pattern);
                int column = 0;
                int row = 0;
                foreach (Match match in matches)
                {
                    string principalQuantumNumber = match.Groups[1].Value;
                    string sublevel = match.Groups[2].Value;
                    string electrons = match.Groups[3].Value;
                    row = Math.Max(int.Parse(principalQuantumNumber), row);
                    column += int.Parse(electrons);
                }

                if (row == 1 && column != 1) column += 16;
                else if (row <= 3 && column >= 3) column += 10;

                var atomicNumber = int.Parse(element.atomic_number);

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
            if (element.category == "Nonmetal")
                element.color = "ab91ff";
            else if (element.category == "Noble gas")
                element.color = "91dfff";
            else if (element.category == "Alkali metal")
                element.color = "ffc591";
            else if (element.category == "Alkaline earth metal")
                element.color = "ffdf91";
            else if (element.category == "Metalloid")
                element.color = "92ff9f";
            else if (element.category == "Halogen")
                element.color = "f991ff";
            else if (element.category == "Post-transition metal")
                element.color = "fff991";
            else if (element.category == "Transition metal")
                element.color = "ecff91";
            else if (element.category == "Lanthanide")
                element.color = "d2ff91";
            else if (element.category == "Actinide")
                element.color = "b8ff91";
            else if (element.category == "Unknown")
                element.color = "c8c8c8";

            return element;
        }

        /// <summary>
        /// Load elements from database
        /// </summary>
        /// <returns></returns>
        private List<ElementModel> LoadElement()
        {
            var elements = SQLiteDataProvider.Instance.ExecuteQuery<ElementModel>("SELECT * FROM Elements");

            // Add Lanthanide group and Actinide group
            elements.Add(new ElementModel
            {
                atomic_number = "57 - 71",
                name = "Lanthanides",
                symbol = "La - Lu",
                phase = "Solid",
                color = "",
                category = "Lanthanide",
                discovery_year = "1838",
                row = 6,
                column = 3,
            });
            elements.Add(new ElementModel
            {
                atomic_number = "89 - 103",
                name = "Actinides",
                symbol = "Ac - Lr",
                phase = "Solid",
                color = "",
                category = "Actinide",
                discovery_year = "1940",
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
