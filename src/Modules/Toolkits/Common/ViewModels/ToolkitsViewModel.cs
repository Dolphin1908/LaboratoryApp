using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Toolkits.CalculatorFunction.Views;

namespace LaboratoryApp.src.Modules.Toolkits.Common.ViewModels
{
    public class ToolkitsViewModel : BaseViewModel
    {
        #region commands
        public ICommand OpenCalculatorCommand { get; set; }
        #endregion

        public ToolkitsViewModel()
        {
            OpenCalculatorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                CalculatorWindow calculatorWindow = new CalculatorWindow();
                calculatorWindow.Show();
            });
        }
    }
}
