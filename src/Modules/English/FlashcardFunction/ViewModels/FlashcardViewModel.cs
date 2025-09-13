using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Services.English.FlashcardFunction;
using LaboratoryApp.src.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels
{
    public class FlashcardViewModel : BaseViewModel
    {
        private string _word;
        private string _pos;
        private string _meaning;
        private string _example;
        private string _note;
        private Flashcard _flashcard;
        private Action<Flashcard> _flashcardCommandCallback;
        private IServiceProvider _serviceProvider;

        private string _name;
        private string _description;
        private FlashcardSet _originalSet;
        private Action<FlashcardSet> _updateCallback;

        #region Commands
        public ICommand UpdateFlashcardCommand { get; set; } // Command to edit a flashcard
        public ICommand UpdateFlashcardSetCommand { get; set; } // Command to edit a flashcard set
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

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        #endregion

        /// <summary>
        /// Constructor cho việc cập nhật một bộ thẻ flashcard.
        /// </summary>
        /// <param name="flashcardSet"></param>
        /// <param name="updateSet"></param>
        public FlashcardViewModel(FlashcardSet flashcardSet, 
                                  Action<FlashcardSet> updateSet)
        {
            _originalSet = flashcardSet;
            _updateCallback = updateSet;

            Name = flashcardSet.Name;
            Description = flashcardSet.Description;

            UpdateFlashcardSetCommand = new RelayCommand<object>((p) => true, (p) => UpdateSet(p));
        }

        /// <summary>
        /// Constructor cho việc thêm/sửa một thẻ flashcard cụ thể.
        /// </summary>
        /// <param name="flashcard"></param>
        /// <param name="flashcardCommand"></param>
        public FlashcardViewModel(IServiceProvider serviceProvider,
                                  Flashcard flashcard, 
                                  Action<Flashcard> flashcardCommand, 
                                  Func<DictionaryWindow> dictionaryWindowFactory)
        {
            _serviceProvider = serviceProvider;
            _flashcard = flashcard;
            _flashcardCommandCallback = flashcardCommand;

            if (_flashcard != null)
            {
                LoadUI();
            }

            UpdateFlashcardCommand = new RelayCommand<object>((p) => true, (p) => UpdateFlashcard(p));
            OpenDictionaryWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                if (window.DataContext is DictionaryViewModel vm && vm is IAsyncInitializable init)
                {
                    // Initialize the dictionary window asynchronously
                    _ = init.InitializeAsync();
                }
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
            _flashcard.LastReviewed = DateTime.Now;
            _flashcard.NextReview = DateTime.Now;
            _flashcard.ReviewCount = 0;
            _flashcard.CorrectStreak = 0;
            _flashcard.IsLearned = false;

            _flashcardCommandCallback?.Invoke(_flashcard);

            if (window is Window win)
            {
                win.Close();
            }
        }
        private void UpdateSet(object window)
        {
            _originalSet.Name = Name;
            _originalSet.Description = Description;

            _updateCallback?.Invoke(_originalSet);

            if (window is Window win)
                win.Close();
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
