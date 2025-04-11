using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.Models.English;
using LaboratoryApp.Services;
using LaboratoryApp.Views.English.SubWin;

namespace LaboratoryApp.ViewModels.English.SubWin
{
    public class AddFlashcardViewModel : BaseViewModel
    {
        private string _word;
        private string _pos;
        private string _meaning;
        private string _example;
        private string _note;
        private FlashcardModel _newFlashcard;
        private Action<FlashcardModel> _addFlashcardCallback;

        #region Commands
        public ICommand AddFlashcardCommand { get; set; } // Command to add a flashcard
        public ICommand OpenDictionaryWindowCommand { get; set; } // Command to open the dictionary window
        #endregion

        #region Properties
        public string Word
        {
            get => _word;
            set
            {
                _word = value;
                OnPropertyChanged(nameof(Word));
            }
        }
        public string Pos
        {
            get => _pos;
            set
            {
                _pos = value;
                OnPropertyChanged(nameof(Pos));
            }
        }
        public string Meaning
        {
            get => _meaning;
            set
            {
                _meaning = value;
                OnPropertyChanged(nameof(Meaning));
            }
        }
        public string Example
        {
            get => _example;
            set
            {
                _example = value;
                OnPropertyChanged(nameof(Example));
            }
        }
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }
        #endregion

        public AddFlashcardViewModel(Action<FlashcardModel> addFlashcard)
        {
            _newFlashcard = new FlashcardModel();
            _addFlashcardCallback = addFlashcard;

            AddFlashcardCommand = new RelayCommand<object>((p) => true, (p) => AddFlashcard(p));
            OpenDictionaryWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new DictionaryWindow
                {
                    DataContext = new DictionaryViewModel()
                };
                window.Show();
            });
        }

        private void AddFlashcard(object window)
        {
            _newFlashcard.Word = Word;
            _newFlashcard.Pos = Pos;
            _newFlashcard.Meaning = Meaning;
            _newFlashcard.Example = Example;
            _newFlashcard.Note = Note;

            _addFlashcardCallback?.Invoke(_newFlashcard);

            if (window is Window win)
            {
                win.Close();
            }    
        }
    }
}
