using LaboratoryApp.Models.English;
using LaboratoryApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.ViewModels.English.SubWin
{
    class UpdateFlashcardSetViewModel : BaseViewModel
    {
        private string _name;
        private string _description;
        private FlashcardSet _originalSet;
        private Action<FlashcardSet> _updateCallback;
        private FlashcardService _flashcardService;

        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(name));
            }
        }

        public string description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(description));
            }
        }

        #region Commands
        public ICommand UpdateFlashcardSetCommand { get; set; }
        #endregion

        public UpdateFlashcardSetViewModel(FlashcardSet flashcardSet, Action<FlashcardSet> updateSet)
        {
            _originalSet = flashcardSet;
            _updateCallback = updateSet;

            name = flashcardSet.name;
            description = flashcardSet.description;

            UpdateFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => UpdateSet(p));
        }

        private void UpdateSet(object window)
        {
            _originalSet.name = name;
            _originalSet.description = description;

            _updateCallback?.Invoke(_originalSet);

            if (window is Window win)
                win.Close();
        }
    }
}
