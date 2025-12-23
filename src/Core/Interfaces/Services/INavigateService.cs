using System.Windows.Controls;

namespace LaboratoryApp.src.Core.Interfaces.Services
{
    public interface INavigateService
    {
        void Initialize(Frame mainFrame);
        void NavigateTo(Page page);
        void NavigateBack();
        void NavigateToAndClearHistory(Page page);
    }
}
