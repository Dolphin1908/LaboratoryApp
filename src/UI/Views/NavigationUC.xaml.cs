using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaboratoryApp.src.UI.Views
{
    /// <summary>
    /// Interaction logic for NavigationUC.xaml
    /// </summary>
    public partial class NavigationUC : UserControl
    {
        public NavigationUC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Không hiểu sao phải dùng sự kiện Loaded mới chỉnh font size của Hamburger Button được
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RootNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            if (RootNavigation.Template.FindName("PART_ToggleButton", RootNavigation) is Button toggleButton)
            {
                toggleButton.FontSize = 24;
                toggleButton.FontWeight = FontWeights.ExtraBold;
                toggleButton.Margin = new Thickness(9, 0, 0, 0);
                toggleButton.Padding = new Thickness(0);
            }    
        }
    }
}
