using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LaboratoryApp.Support.Interface;
using LaboratoryApp.ViewModels.English.SubWin;
using LaboratoryApp.Views.English.SubWin;

namespace LaboratoryApp.ViewModels.English
{
    class EnglishMainWindowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        #region Commands
        public ICommand OpenDictionaryCommand { get; set; } // Open Dictionary
        #endregion

        public EnglishMainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Open Dictionary Window
                var dictionaryWindow = new DictionaryWindow
                {
                    DataContext = new DictionaryViewModel()
                };
                dictionaryWindow.Show();
            });
        }
    }
}
