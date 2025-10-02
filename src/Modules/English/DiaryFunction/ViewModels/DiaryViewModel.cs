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

using Microsoft.Extensions.DependencyInjection;

using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DiaryFunction.DTOs;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Services.AI;
using LaboratoryApp.src.Services.English;
using LaboratoryApp.src.Shared.Interface;
using LaboratoryApp.src.Services.Counter;
using LaboratoryApp.src.Constants;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryViewModel : BaseViewModel
    {
        private readonly IAIService _aiService;
        private readonly IEnglishService _englishService;
        private readonly ICounterService _counterService;
        private readonly IServiceProvider _serviceProvider;

        private bool _isPopupOpen = false;
        private bool _isEdit = false;
        private bool _isPublic = false;
        private string _selectedColor = "#000000";
        private double _selectedFontSize = 13;
        private TextAlignment _selectedAlignment = TextAlignment.Left;
        private string _title;
        private FlowDocument _boundDocument { get; set; }

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
        public bool IsPublic
        {
            get { return _isPublic; }
            set
            {
                _isPublic = value;
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
        public TextAlignment SelectedAlignment
        {
            get { return _selectedAlignment; }
            set
            {
                _selectedAlignment = value;
                OnPropertyChanged();
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
        public ICommand SelectAlignCommand { get; set; }
        public ICommand OpenDictionaryCommand { get; set; }
        public ICommand EditWithAICommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor for creating a new diary entry
        /// </summary>
        /// <param name="englishService"></param>
        /// <param name="englishDataCache"></param>
        public DiaryViewModel(IAIService aiService,
                              IEnglishService englishService, 
                              ICounterService counterService,
                              IServiceProvider serviceProvider)
        {
            _aiService = aiService;
            _englishService = englishService;
            _counterService = counterService;
            _serviceProvider = serviceProvider;

            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };

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

            SelectAlignCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (p == null)
                    return;

                var align = p as string;

                if (align == "Left")
                    SelectedAlignment = TextAlignment.Left;
                else if (align == "Center")
                    SelectedAlignment = TextAlignment.Center;
                else if (align == "Right")
                    SelectedAlignment = TextAlignment.Right;
                else if (align == "Justify")
                    SelectedAlignment = TextAlignment.Justify;
            });

            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                if (window.DataContext is DictionaryViewModel vm && vm is IAsyncInitializable init)
                {
                    // Initialize the dictionary window asynchronously
                    _ = init.InitializeAsync();
                }
                window.Show();
            });

            EditWithAICommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (MessageBox.Show("Bạn có chấp nhận chia sẻ nội dung với AI không?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;

                var textRange = new TextRange(BoundDocument.ContentStart, BoundDocument.ContentEnd);
                DiaryResultDTO result = await _aiService.EditDiaryWithAIAsync(Title, textRange.Text);
                
                if (MessageBox.Show(result.Content, result.Title, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Title = result.Title;
                    BoundDocument = FlowDocumentSerializer.DeserializeFromString(result.Content);
                }    
            });

            SaveCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    SaveDiary();

                if (p is DiaryWindow window)
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
        public DiaryViewModel(IAIService aiService,
                              IEnglishService englishService,
                              ICounterService counterService,
                              IServiceProvider serviceProvider,
                              DiaryContent diaryContent)
        {
            _aiService = aiService;
            _englishService = englishService;
            _counterService = counterService;
            _serviceProvider = serviceProvider;

            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };

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

            SelectAlignCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (p == null)
                    return;

                var align = p as string;

                if (align == "Left")
                    SelectedAlignment = TextAlignment.Left;
                else if (align == "Center")
                    SelectedAlignment = TextAlignment.Center;
                else if (align == "Right")
                    SelectedAlignment = TextAlignment.Right;
                else if (align == "Justify")
                    SelectedAlignment = TextAlignment.Justify;
            });

            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                if (window.DataContext is DictionaryViewModel vm && vm is IAsyncInitializable init)
                {
                    // Initialize the dictionary window asynchronously
                    _ = init.InitializeAsync();
                }
                window.Show();
            });

            EditWithAICommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (MessageBox.Show("Bạn có chấp nhận chia sẻ nội dung với AI không?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;

                var textRange = new TextRange(BoundDocument.ContentStart, BoundDocument.ContentEnd);
                DiaryResultDTO result = await _aiService.EditDiaryWithAIAsync(Title, textRange.Text);

                if (MessageBox.Show(result.Content, result.Title, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Title = result.Title;
                    BoundDocument = FlowDocumentSerializer.DeserializeFromString(result.Content);
                }
            });

            SaveCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (MessageBox.Show("Do you want to save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    SaveChanges(diaryContent);

                if (p is DiaryWindow window)
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
                Id = _counterService.GetNextId(CollectionName.Diaries),
                Title = this.Title ?? "Untitled",
                IsPublic = this.IsPublic,
                UserId = AuthenticationCache.CurrentUser?.Id ?? 0,
                ContentBytes = data,
                ContentFormat = "XamlPackage",
            };

            try
            {
                _englishService.AddDiary(entry);
                EnglishDataCache.AllDiaries.Add(entry);
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
            diaryContent.IsPublic = this.IsPublic;
            diaryContent.ContentBytes = data;
            diaryContent.UpdatedAt = DateTime.UtcNow;

            try
            {
                // Cập nhật trong database
                _englishService.UpdateDiary(diaryContent);

                // Cập nhật trong cache
                var index = EnglishDataCache.AllDiaries.FindIndex(d => d.Id == diaryContent.Id);
                if (index >= 0) 
                    EnglishDataCache.AllDiaries[index] = diaryContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating diary: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
