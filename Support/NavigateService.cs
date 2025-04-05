using LaboratoryApp.Support.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LaboratoryApp.Support
{
    public class NavigateService : INavigationService
    {
        private Frame _mainFrame;

        public NavigateService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void NavigateTo(Page page)
        {
            _mainFrame.Navigate(page);
        }
    }
}
