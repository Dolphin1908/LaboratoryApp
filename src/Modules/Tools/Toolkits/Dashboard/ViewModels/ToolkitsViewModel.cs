using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Toolkits.CalculatorFunction.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.Toolkits.Dashboard.ViewModels
{
    public class ToolkitsViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;

        #region commands
        public ICommand OpenCalculatorCommand { get; set; }
        #endregion

        public ToolkitsViewModel(IServiceProvider serviceProvider,
                                 IDialogService dialogService)
        {
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;

            OpenCalculatorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                CalculatorWindow calculatorWindow = _serviceProvider.GetRequiredService<CalculatorWindow>();
                _dialogService.ShowCenterOwner(calculatorWindow);
            });
        }
    }
}
