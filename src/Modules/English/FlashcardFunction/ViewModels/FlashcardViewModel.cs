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
        private readonly IServiceProvider _serviceProvider;
        private readonly IFlashcardService _flashcardService;

        private readonly Flashcard _card;
        private readonly long _setId;
        private readonly bool _isNewCard;

        private string _word;
        private string _pos;
        private string _meaning;
        private string _example;
        private string _note;

        #region Commands
        public ICommand SaveCommand { get; set; } // Command to edit a flashcard
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
        public string Title => _isNewCard ? "Thêm thẻ mới" : "Chỉnh sửa thẻ";
        #endregion

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="flashcard"></param>
        /// <param name="flashcardCommand"></param>
        public FlashcardViewModel(IServiceProvider serviceProvider,
                                  IFlashcardService flashcardService,
                                  long setId,
                                  Flashcard card)
        {
            _serviceProvider = serviceProvider;
            _flashcardService = flashcardService;

            _setId = setId;
            _card = card; // Đây có thể là card mới (Id=0) hoặc card đã tồn tại
            _flashcardService = flashcardService;
            _isNewCard = card.Id == 0;

            Word = _card.Word;
            Pos = _card.Pos;
            Meaning = _card.Meaning;
            Example = _card.Example;
            Note = _card.Note;

            SaveCommand = new RelayCommand<object>((p) => CanSave(), (p) => SaveChanges(p));
            OpenDictionaryWindowCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                window.Show();
            });
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Word) && !string.IsNullOrWhiteSpace(Meaning);

        private void SaveChanges(object parameter)
        {
            // Cập nhật thông tin từ UI vào đối tượng model
            _card.Word = this.Word;
            _card.Meaning = this.Meaning;
            _card.Example = this.Example;
            _card.Note = this.Note;
            _card.Pos = this.Pos;

            // ✅ ViewModel tự quyết định gọi Add hay Update dựa vào trạng thái
            if (_isNewCard)
            {
                _flashcardService.AddCardToSet(_setId, _card);
            }
            else
            {
                _flashcardService.UpdateCardInSet(_setId, _card);
            }

            if (parameter is Window win)
            {
                win.Close();
            }    
        }
    }
}
