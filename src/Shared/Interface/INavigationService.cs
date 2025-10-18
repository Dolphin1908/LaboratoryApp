using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LaboratoryApp.src.Shared.Interface
{
    public interface INavigationService
	{
        void Initialize(Frame mainFrame);
        void NavigateTo(Page page);
        void NavigateBack();
    }
}
