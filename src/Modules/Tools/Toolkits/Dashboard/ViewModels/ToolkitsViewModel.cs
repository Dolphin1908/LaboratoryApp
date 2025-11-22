using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Toolkits.CalculatorFunction.Views;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.Toolkits.Dashboard.ViewModels
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
