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
using System.Windows.Shapes;

using LaboratoryApp.ViewModels.English.Dictionary.SubWin;

namespace LaboratoryApp.Views.English.Dictionary.SubWin
{
    /// <summary>
    /// Interaction logic for Dictionary.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {
        public DictionaryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SearchBox.Focus();
        }
    }
}
