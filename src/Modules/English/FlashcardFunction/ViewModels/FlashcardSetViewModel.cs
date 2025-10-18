using LaboratoryApp.src.Core.Models.English.FlashcardFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Services.English.FlashcardFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.English.FlashcardFunction.ViewModels
{
    public class FlashcardSetViewModel : BaseViewModel
    {
        private readonly IFlashcardService _flashcardService;
        private readonly FlashcardSet _originalSet;

        private string _name;
        private string _description;

        #region Commands
        public ICommand SaveCommand { get; set; } // Command to edit a flashcard set
        #endregion

        #region Properties
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


        public FlashcardSetViewModel(IFlashcardService flashcardService,
                                     FlashcardSet flashcardSet)
        {
            _flashcardService = flashcardService;
            _originalSet = flashcardSet;

            Name = _originalSet.Name;
            Description = _originalSet.Description;

            SaveCommand = new RelayCommand<object>((p) => CanSave(), (p) => SaveChanges(p));
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Name);

        private void SaveChanges(object parameter)
        {
            // Cập nhật thông tin vào đối tượng gốc
            _originalSet.Name = this.Name;
            _originalSet.Description = this.Description;

            // ✅ Gọi service để lưu, không cần callback
            _flashcardService.UpdateSet(_originalSet);

            if (parameter is Window win)
            {
                win.Close();
            }
        }
    }
}
