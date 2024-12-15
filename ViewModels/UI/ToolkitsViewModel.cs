using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LaboratoryApp.ViewModels.SubWin;
using LaboratoryApp.Views.SubWin;

namespace LaboratoryApp.ViewModels.UI
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
