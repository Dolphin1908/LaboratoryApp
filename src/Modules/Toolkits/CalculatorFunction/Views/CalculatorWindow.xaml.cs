﻿using System;
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

using LaboratoryApp.src.Modules.Toolkits.CalculatorFunction.ViewModels;

namespace LaboratoryApp.src.Modules.Toolkits.CalculatorFunction.Views
{
    /// <summary>
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        public CalculatorWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is CalculatorViewModel viewModel)
            {
                viewModel.Reset(); // Gọi phương thức reset
            }
        }
    }
}
