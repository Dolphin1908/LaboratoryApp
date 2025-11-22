using System.Windows.Controls;

namespace LaboratoryApp.src.Shared.Interfaces
{
    public interface INavigationService
    {
        void Initialize(Frame mainFrame);
        void NavigateTo(Page page);
        void NavigateBack();
    }
}
