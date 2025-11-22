using LaboratoryApp.src.Shared.Interfaces;
using System.Windows.Controls;

namespace LaboratoryApp.src.Services.UI
{
    public class NavigateService : INavigationService
    {
        private Frame? _mainFrame;

        public NavigateService()
        {
            // Do nothing
        }

        public void Initialize(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void NavigateTo(Page page)
        {
            _mainFrame.Navigate(page);
        }

        public void NavigateBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }
    }
}
