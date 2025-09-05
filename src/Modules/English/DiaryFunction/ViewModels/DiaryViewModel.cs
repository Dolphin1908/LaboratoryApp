using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Services.English;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryViewModel : BaseViewModel
    {
        private readonly IEnglishService _englishService;
        private readonly EnglishDataCache _englishDataCache;

        private bool _isPopupOpen = false;
        private bool _isEdit = false;
        private string _selectedColor = "#000000";
        private double _selectedFontSize = 12;
        private string _title;
        private string _selectedMode = "private";
        private FlowDocument _boundDocument { get; set; }

        public ObservableCollection<string> Modes { get; }
        public ObservableCollection<double> FontSizes { get; }

        #region Properties
        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                _isEdit = value;
                OnPropertyChanged();
            }
        }
        public string SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }
        public double SelectedFontSize
        {
            get { return _selectedFontSize; }
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public string SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                _selectedMode = value;
                OnPropertyChanged();
            }
        }
        public FlowDocument BoundDocument
        {
            get { return _boundDocument; }
            set
            {
                _boundDocument = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand OpenFontColorCommand { get; set; }
        public ICommand SelectColorCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor for creating a new diary entry
        /// </summary>
        /// <param name="englishService"></param>
        /// <param name="englishDataCache"></param>
        public DiaryViewModel(IEnglishService englishService,
                              EnglishDataCache englishDataCache)
        {
            _englishService = englishService;
            _englishDataCache = englishDataCache;

            Modes = new ObservableCollection<string>() { "private", "public" };
            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };
            SelectedFontSize = 12;

            BoundDocument = new FlowDocument();

            #region Commands
            OpenFontColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IsPopupOpen = !IsPopupOpen;
            });

            SelectColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                SelectedColor = p as string;
                IsPopupOpen = !IsPopupOpen;
            });

            SaveCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    SaveDiary();

                if (p is AddDiaryWindow window)
                    window.Close();
            });
            #endregion
        }

        /// <summary>
        /// Constructor for editing an existing diary entry
        /// </summary>
        /// <param name="englishService"></param>
        /// <param name="englishDataCache"></param>
        /// <param name="diaryContent"></param>
        public DiaryViewModel(IEnglishService englishService,
                              EnglishDataCache englishDataCache,
                              DiaryContent diaryContent)
        {
            _englishService = englishService;
            _englishDataCache = englishDataCache;

            Modes = new ObservableCollection<string>() { "private", "public" };
            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };
            SelectedFontSize = 12;

            #region Commands
            OpenFontColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IsPopupOpen = !IsPopupOpen;
            });

            SelectColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                SelectedColor = p as string;
                IsPopupOpen = !IsPopupOpen;
            });

            SaveCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    SaveChanges(diaryContent);

                if (p is AddDiaryWindow window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            });
            #endregion
        }

        private void SaveDiary()
        {
            // Lưu nhật ký
            if (BoundDocument == null)
                BoundDocument = new FlowDocument();

            byte[] data = FlowDocumentSerializer.SerializeToBytes(BoundDocument);

            var entry = new DiaryContent
            {
                Id = _englishDataCache.AllDiaries.Count + 1,
                Title = this.Title ?? "Untitled",
                Mode = this.SelectedMode,
                UserId = AuthenticationCache.CurrentUser?.Id ?? 0,
                ContentBytes = data,
                ContentFormat = "XamlPackage",
            };

            try
            {
                _englishService.AddDiary(entry);
                _englishDataCache.AllDiaries.Add(entry);
                MessageBox.Show("Diary saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving diary: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveChanges(DiaryContent diaryContent)
        {
            // Cập nhật nhật ký
            if (BoundDocument == null)
                BoundDocument = new FlowDocument();

            // Chuyển FlowDocument thành byte array
            byte[] data = FlowDocumentSerializer.SerializeToBytes(BoundDocument);

            // Cập nhật các trường
            diaryContent.Title = this.Title ?? "Untitled";
            diaryContent.Mode = this.SelectedMode;
            diaryContent.ContentBytes = data;
            diaryContent.UpdatedAt = DateTime.UtcNow;

            try
            {
                // Cập nhật trong database
                _englishService.UpdateDiary(diaryContent);

                // Cập nhật trong cache
                var index = _englishDataCache.AllDiaries.FindIndex(d => d.Id == diaryContent.Id);
                if (index >= 0) 
                    _englishDataCache.AllDiaries[index] = diaryContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating diary: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
