﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.Views;

namespace LaboratoryApp.src.Modules.Chemistry.PeriodicFunction.ViewModels
{
    public class ElementInfoViewModel : BaseViewModel
    {
        #region Commands
        public ICommand ElementCellClickedCommand { get; set; }
        public ICommand CloseElementInfoCommand { get; set; }
        #endregion

        private Element _element;
        public Element Element
        {
            get { return _element; }
            set
            {
                _element = value;
                OnPropertyChanged();
            }
        }

        // Get element row and column for periodic table
        public int Row => Element.Row - 1;
        public int Column => Element.Column - 1;

        public ElementInfoViewModel()
        {
            // Default constructor
        }

        public ElementInfoViewModel(Element element)
        {
            Element = element;

            // Handle element cell clicked
            ElementCellClickedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new ElementInfo
                {
                    DataContext = this
                };
                window.Title = Element.Name;
                window.ShowDialog();
            });

            // Handle close window
            CloseElementInfoCommand = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
        }
    }
}
