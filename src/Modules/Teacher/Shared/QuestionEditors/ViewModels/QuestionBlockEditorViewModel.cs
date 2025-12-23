using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels.Items;
using LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.Views;
using LaboratoryApp.src.Shared.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.ViewModels
{
    public class QuestionBlockEditorViewModel : ExerciseContentItemViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAssetService _assetService;
        private readonly IDialogService _dialogService;
        private readonly IFormatConversionService _formatService;
        private Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel> _questionEditorVmFactory;

        private bool _isPopupOpen = false;
        private string _selectedColor = "#000000";
        private double _selectedFontSize = 13;
        private TextAlignment _selectedAlignment = TextAlignment.Left;
        private bool _isSave = false;

        private string _blockTitle = string.Empty;
        private string _xamlDocument = string.Empty;

        public ObservableCollection<double> FontSizes { get; }

        #region Commands
        public ICommand SaveCommand { get; set; }
        public ICommand AddChildQuestionCommand { get; set; }
        public ICommand DeleteChildQuestionCommand { get; set; }
        public ICommand CancelAddBlockCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }

        // Popup and formatting commands
        public ICommand OpenFontColorCommand { get; set; }
        public ICommand OpenMoreFontColorCommand { get; set; }
        public ICommand SelectMoreColorCommand { get; set; }
        public ICommand CancelMoreColorCommand { get; set; }
        public ICommand SelectColorCommand { get; set; }
        public ICommand SelectAlignCommand { get; set; }
        #endregion

        #region Configure
        public override string IconKind => "FileDocumentMultipleOutline";
        public override bool IsBlock => true;
        public override ObservableCollection<ExerciseContentItemViewModel>? Children { get; set; }
        #endregion

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
        public string BlockTitle
        {
            get => _blockTitle;
            set
            {
                _blockTitle = value;
                OnPropertyChanged(nameof(BlockTitle));
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

        public QuestionBlock Model { get; }
        public QuestionBlockEditorViewModel(IServiceProvider serviceProvider,
                                            IAssetService assetService,
                                            IDialogService dialogService,
                                            IFormatConversionService formatService,
                                            QuestionBlock model,
                                            Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel> questionEditorVmFactory)
        {
            _serviceProvider = serviceProvider;
            _assetService = assetService;
            _dialogService = dialogService;
            _formatService = formatService;
            _questionEditorVmFactory = questionEditorVmFactory;
            Model = model;
            Children = new ObservableCollection<ExerciseContentItemViewModel>();

            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };

            InitializeCommands();
        }

        /// <summary>
        /// Khởi tạo commands
        /// </summary>
        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand<object>(p => true, async p =>
            {
                _isSave = true;
                Model.AssetId = (await _assetService.GetOrAddTextAssetAsync(XamlDocument, AuthenticationCache.CurrentAuthentication!.User.Id)).Id;

                if (p is QuestionBlockEditorWindow window)
                {
                    Model.Title = _formatService.ConvertPlainTextToXaml(BlockTitle);
                    window.DialogResult = true;
                    window.Close();
                }
            });
            AddChildQuestionCommand = new RelayCommand<object>(p => true, p => AddChildQuestion());
            DeleteChildQuestionCommand = new RelayCommand<object>(p => true, p => { });
            CancelAddBlockCommand = new RelayCommand<object>(p => true, async p => await CancelAddBlock(p));
            WindowClosingCommand = new RelayCommand<object>(p => true, async p =>
            {
                if (Children != null && Children.Count > 0 && _isSave == false)
                {
                    foreach (var item in Children)
                    {
                        await DeleteAsset(item);
                    }
                }
            });

            OpenFontColorCommand = new RelayCommand<object>((p) => true, (p) => TogglePopupOpen());
            OpenMoreFontColorCommand = new RelayCommand<object>((p) => true, (p) => ShowMoreColorsWindow());
            SelectMoreColorCommand = new RelayCommand<object>((p) => true, (p) => SelectMoreColor(p));
            CancelMoreColorCommand = new RelayCommand<object>((p) => true, (p) => CancelMoreColor(p));
            SelectColorCommand = new RelayCommand<object>((p) => true, (p) => SelectColor(p));
            SelectAlignCommand = new RelayCommand<object>((p) => true, (p) => SelectAlignment(p));
        }

        private void AddChildQuestion()
        {
            if (_questionEditorVmFactory != null)
            {
                var newQuestion = new Question
                {
                    Type = QuestionType.MultipleChoice
                };

                var vm = _questionEditorVmFactory(_assetService, _dialogService, _formatService, newQuestion);
                var window = _serviceProvider.GetRequiredService<QuestionEditorWindow>();
                window.DataContext = vm;
                if (_dialogService.ShowDialogCenterOwner(window) == true)
                {
                    Children!.Add(vm);
                    _dialogService.ShowMessage(vm.Model.Title, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

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

        private async Task CancelAddBlock(object parameter)
        {
            if (_dialogService.ShowMessage("Bạn có muốn thoát không?", "Hủy tạo khối", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (Children != null && Children.Count > 0)
                {
                    foreach (var item in Children)
                    {
                        await DeleteAsset(item);
                    }
                }

                if (parameter is QuestionBlockEditorWindow window)
                {
                    window.DialogResult = false;
                    window.Close();
                }
            }
        }

        /// <summary>
        /// Thu hồi tài nguyên (nếu có) khi xóa câu hỏi
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task DeleteAsset(ExerciseContentItemViewModel item)
        {
            if (item is QuestionEditorViewModel qVm)
            {
                if (qVm.Model.AssetId > 0)
                {
                    await _assetService.ReleaseAssetAsync(qVm.Model.AssetId);
                }

                foreach (var ans in qVm.Model.AnswerOptions)
                {
                    if (ans.AssetId > 0)
                        await _assetService.ReleaseAssetAsync(ans.AssetId);
                }
            }
        }
    }
}
