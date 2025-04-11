using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using LaboratoryApp.Models.English;
using LaboratoryApp.Services;
using LaboratoryApp.Views.English.SubWin;

namespace LaboratoryApp.ViewModels.English.SubWin
{
    public class EditFlashcardViewModel : BaseViewModel
    {
        private string _word;
        private string _pos;
        private string _meaning;
        private string _example;
        private string _note;
        private FlashcardModel _editedFlashcard;
        private Action<FlashcardModel> _editFlashcardCallback;

        #region Commands
        public ICommand EditFlashcardCommand { get; set; } // Command to add a flashcard
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

        public EditFlashcardViewModel(FlashcardModel flashcard, Action<FlashcardModel> editFlashcard)
        {
            _editedFlashcard = flashcard;
            _editFlashcardCallback = editFlashcard;

            LoadUI();

            EditFlashcardCommand = new RelayCommand<object>((p) => true, (p) => EditFlashcard(p));
            OpenDictionaryWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = new DictionaryWindow
                {
                    DataContext = new DictionaryViewModel()
                };
                window.Show();
            });
        }

        private void EditFlashcard(object window)
        {
            _editedFlashcard.Word = Word;
            _editedFlashcard.Pos = Pos;
            _editedFlashcard.Meaning = Meaning;
            _editedFlashcard.Example = Example;
            _editedFlashcard.Note = Note;

            _editFlashcardCallback?.Invoke(_editedFlashcard);

            if (window is Window win)
            {
                win.Close();
            }
        }

        private void LoadUI()
        {
            Word = _editedFlashcard.Word;
            Pos = _editedFlashcard.Pos;
            Meaning = _editedFlashcard.Meaning;
            Example = _editedFlashcard.Example;
            Note = _editedFlashcard.Note;
        }
    }
}
