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

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Models.English.DiaryFunction.DTOs;
using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Data.Providers.English.DiaryFunction;

using LaboratoryApp.src.Modules.English.DiaryFunction.Views;
using LaboratoryApp.src.Modules.English.DictionaryFunction.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;

using LaboratoryApp.src.Services.English.DiaryFunction;

using LaboratoryApp.src.Services.Helper.AI;
using LaboratoryApp.src.Services.Helper.Counter;

using LaboratoryApp.src.Shared.Views;
using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.English.DiaryFunction.ViewModels
{
    public class DiaryViewModel : BaseViewModel
    {
        private readonly IAIService _aiService;
        private readonly IDiaryService _diaryService;
        private readonly IServiceProvider _serviceProvider;

        private bool _isPopupOpen = false;
        private bool _isEdit = false;
        private bool _isPublic = false;
        private string _selectColor = "#000000";
        private string _selectedColor = "#000000";
        private double _selectedFontSize = 13;
        private TextAlignment _selectedAlignment = TextAlignment.Left;
        private string? _title;
        private FlowDocument? _boundDocument { get; set; }
        private DiaryContent? _editingDiary;

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
        public string SelectColor
        {
            get { return _selectColor; }
            set
            {
                _selectColor = value;
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
            get => _title ?? string.Empty;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public FlowDocument BoundDocument
        {
            get => _boundDocument ?? new FlowDocument();
            set
            {
                _boundDocument = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand OpenFontColorCommand { get; set; }
        public ICommand OpenMoreFontColorCommand { get; set; }
        public ICommand SelectMoreColorCommand { get; set; }
        public ICommand CancelMoreColorCommand { get; set; }
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
        public DiaryViewModel(IServiceProvider serviceProvider, 
                              IAIService aiService,
                              IDiaryService diaryService,
                              DiaryContent? diaryToEdit = null)
        {
            _aiService = aiService;
            _diaryService = diaryService;
            _serviceProvider = serviceProvider;

            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };

            if (diaryToEdit != null)
            {
                IsEdit = true;
                _editingDiary = diaryToEdit;

                Title = diaryToEdit.Title;
                IsPublic = diaryToEdit.IsPublic;
                BoundDocument = FlowDocumentSerializer.DeserializeFromBytes(diaryToEdit.ContentBytes);
            }
            else
            {
                IsEdit = false;
                BoundDocument = new FlowDocument();
            }

            #region Commands
            OpenFontColorCommand = new RelayCommand<object>((p) => true, (p) => IsPopupOpen = !IsPopupOpen);

            OpenMoreFontColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IsPopupOpen = !IsPopupOpen;
                var moreColorWindow = new MoreColorsWindow
                {
                    DataContext = this
                };
                moreColorWindow.ShowDialog();
            });

            SelectMoreColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                SelectedColor = SelectColor;
                if (p is MoreColorsWindow window)
                {
                    SelectColor = "#000000";
                    window.Close();
                }
            });

            CancelMoreColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (p is MoreColorsWindow window)
                {
                    SelectColor = "#000000";
                    window.Close();
                }
            });

            SelectColorCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                SelectedColor = p as string ?? "#000000";
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
                window.Show();
            });

            EditWithAICommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (MessageBox.Show("Bạn có chấp nhận chia sẻ nội dung với AI không?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;

                var textRange = new TextRange(BoundDocument.ContentStart, BoundDocument.ContentEnd);
                DiaryResultDTO? result = await _aiService.EditDiaryWithAIAsync(Title, textRange.Text);

                if (result != null && MessageBox.Show(result.Content, result.Title, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Title = result.Title;
                    BoundDocument = FlowDocumentSerializer.DeserializeFromString(result.Content);
                }
            });

            SaveCommand = new RelayCommand<object>((p) => CanSave(), async (p) => await SaveAsync(p));
            #endregion
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Title);

        private async Task SaveAsync(object parameter)
        {
            if (MessageBox.Show("Bạn có muốn lưu thay đổi?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                if (IsEdit && _editingDiary != null)
                {
                    await _diaryService.UpdateDiaryAsync(_editingDiary, Title, IsPublic, BoundDocument);
                }
                else
                {
                    await _diaryService.CreateDiaryAsync(Title, IsPublic, BoundDocument);
                }

                MessageBox.Show("Lưu thành công!", "Thành công");

                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving diary: {ex.Message}", "Error");
            }
        }
    }
}