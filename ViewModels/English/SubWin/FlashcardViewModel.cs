using LaboratoryApp.Models.English;
using LaboratoryApp.Views.English.SubWin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace LaboratoryApp.ViewModels.English.SubWin
{
    public class FlashcardViewModel : BaseViewModel
    {
        private string _word;
        private string _pos;
        private string _meaning;
        private string _example;
        private string _note;
        private FlashcardModel _flashcard;
        private Action<FlashcardModel> _flashcardCommandCallback;

        #region Commands
        public ICommand UpdateFlashcardCommand { get; set; } // Command to edit a flashcard
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

        public FlashcardViewModel(FlashcardModel flashcard, Action<FlashcardModel> flashcardCommand)
        {
            _flashcard = flashcard;
            _flashcardCommandCallback = flashcardCommand;

            if (_flashcard != null)
            {
                LoadUI();
            }

            UpdateFlashcardCommand = new RelayCommand<object>((p) => true, (p) => UpdateFlashcard(p));
            OpenDictionaryWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new DictionaryWindow
                {
                    DataContext = new DictionaryViewModel()
                };
                window.Show();
            });
        }

        private void UpdateFlashcard(object window)
        {
            _flashcard.Word = Word;
            _flashcard.Pos = Pos;
            _flashcard.Meaning = Meaning;
            _flashcard.Example = Example;
            _flashcard.Note = Note;

            _flashcardCommandCallback?.Invoke(_flashcard);

            if (window is Window win)
            {
                win.Close();
            }
        }

        private void LoadUI()
        {
            Word = _flashcard.Word;
            Pos = _flashcard.Pos;
            Meaning = _flashcard.Meaning;
            Example = _flashcard.Example;
            Note = _flashcard.Note;
        }
    }
}
