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

using LaboratoryApp.src.UI.ViewModels;
using LaboratoryApp.src.Shared;

namespace LaboratoryApp.src.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Frame mainFrame => this.MainFrame;

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}