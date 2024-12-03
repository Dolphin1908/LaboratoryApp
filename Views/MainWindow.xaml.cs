using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LaboratoryApp.Support;
using LaboratoryApp.ViewModels;
using LaboratoryApp.Views.UI;

namespace LaboratoryApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var navigationService = new NavigateService(MainFrame);
            DataContext = new MainWindowViewModel(navigationService);
        }
    }
}