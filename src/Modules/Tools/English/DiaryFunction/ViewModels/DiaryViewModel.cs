using LaboratoryApp.Domain.DTOs.English.DiaryFunction;
using LaboratoryApp.Domain.Interfaces.Services.English;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.English.DiaryFunction;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.DictionaryFunction.Views;
using LaboratoryApp.src.Shared.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Tools.English.DiaryFunction.ViewModels
{
    public class DiaryViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAIService _aiService;
        private readonly IDialogService _dialogService;
        private readonly IDiaryService _diaryService;
        private readonly IFormatConversionService _formatConversionService;

        private bool _isPopupOpen = false;
        private bool _isEdit = false;
        private bool _isPublic = false;
        private string _selectedColor = "#000000";
        private double _selectedFontSize = 13;
        private TextAlignment _selectedAlignment = TextAlignment.Left;
        private string? _title;

        private string _xamlDocument = string.Empty;
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
        public string XamlDocument
        {
            get => _xamlDocument;
            set
            {
                _xamlDocument = value;
                OnPropertyChanged(nameof(XamlDocument));
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
                              IDialogService dialogService,
                              IDiaryService diaryService,
                              IFormatConversionService formatConversionService,
                              DiaryContent? diaryToEdit = null)
        {
            _aiService = aiService;
            _dialogService = dialogService;
            _diaryService = diaryService;
            _formatConversionService = formatConversionService;
            _serviceProvider = serviceProvider;

            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };

            if (diaryToEdit != null)
            {
                IsEdit = true;
                _editingDiary = diaryToEdit;

                Title = diaryToEdit.Title;
                IsPublic = diaryToEdit.IsPublic;
                XamlDocument = _formatConversionService.DeserializeStringFromByteArray(diaryToEdit.ContentBytes);
            }
            else
            {
                IsEdit = false;
                XamlDocument = string.Empty;
            }

            InitializeCommands();
        }

        /// <summary>
        /// Khởi tạo commands
        /// </summary>
        private void InitializeCommands()
        {
            OpenFontColorCommand = new RelayCommand<object>((p) => true, (p) => TogglePopupOpen());
            OpenMoreFontColorCommand = new RelayCommand<object>((p) => true, (p) => ShowMoreColorsWindow());
            SelectMoreColorCommand = new RelayCommand<object>((p) => true, (p) => SelectMoreColor(p));
            CancelMoreColorCommand = new RelayCommand<object>((p) => true, (p) => CancelMoreColor(p));
            SelectColorCommand = new RelayCommand<object>((p) => true, (p) => SelectColor(p));
            SelectAlignCommand = new RelayCommand<object>((p) => true, (p) => SelectAlignment(p));
            OpenDictionaryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var window = _serviceProvider.GetRequiredService<DictionaryWindow>();
                window.Show();
            });
            EditWithAICommand = new RelayCommand<object>((p) => true, async (p) => await EditWithAIAsync());
            SaveCommand = new RelayCommand<object>((p) => CanSave(), async (p) => await SaveAsync(p));
        }

        /// <summary>
        /// Xử lý popup
        /// </summary>
        private void TogglePopupOpen()
        {
            IsPopupOpen = !IsPopupOpen;
        }

        private void ShowMoreColorsWindow()
        {
            IsPopupOpen = !IsPopupOpen;
            var moreColorWindow = new MoreColorsWindow
            {
                DataContext = this
            };
            moreColorWindow.ShowDialog();
        }

        private void SelectMoreColor(object p)
        {

        }

        private void CancelMoreColor(object p)
        {

        }

        private void SelectColor(object p)
        {
            SelectedColor = p as string ?? "#000000";
            IsPopupOpen = !IsPopupOpen;
        }

        private void SelectAlignment(object p)
        {
            if (p == null) return;

            var align = p as string;

            switch (align)
            {
                case "Left":
                    SelectedAlignment = TextAlignment.Left;
                    break;
                case "Center":
                    SelectedAlignment = TextAlignment.Center;
                    break;
                case "Right":
                    SelectedAlignment = TextAlignment.Right;
                    break;
                case "Justify":
                    SelectedAlignment = TextAlignment.Justify;
                    break;
            }
        }

        private async Task EditWithAIAsync()
        {

            if (_dialogService.ShowMessage("Bạn có chấp nhận chia sẻ nội dung với AI không?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;

            string plainText = _formatConversionService.ConvertXamlToPlainText(XamlDocument);
            DiaryResultDTO? result = await _aiService.EditDiaryWithAIAsync(Title, plainText);

            if (result != null && _dialogService.ShowMessage(result.Content, result.Title, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Title = result.Title;
                XamlDocument = _formatConversionService.ConvertPlainTextToXaml(result.Content);
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Title);

        private async Task SaveAsync(object parameter)
        {
            if (_dialogService.ShowMessage("Bạn có muốn lưu thay đổi?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                //byte[] document = FlowDocumentSerializer.SerializeToBytes(BoundDocument);
                byte[] document = _formatConversionService.SerializeStringToByteArray(XamlDocument);

                if (IsEdit && _editingDiary != null)
                {
                    await _diaryService.UpdateDiaryAsync(_editingDiary, Title, IsPublic, document);
                }
                else
                {
                    await _diaryService.CreateDiaryAsync(Title, IsPublic, document);
                }

                _dialogService.ShowMessage("Lưu thành công!", "Thành công");

                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Error saving diary: {ex.Message}", "Error");
            }
        }
    }
}