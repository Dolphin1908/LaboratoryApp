using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Helpers;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels.Items;
using LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.ViewModels.QuestionSpecifics;
using LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.Views;
using LaboratoryApp.src.Shared.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.ViewModels
{
    public class QuestionEditorViewModel : ExerciseContentItemViewModel
    {
        private readonly IAssetService _assetService;
        private readonly IDialogService _dialogService;
        private readonly IFormatConversionService _formatService;

        // Popup and formatting properties
        private bool _isPopupOpen = false;
        private string _selectedColor = "#000000";
        private double _selectedFontSize = 13;
        private TextAlignment _selectedAlignment = TextAlignment.Left;
        public ObservableCollection<double> FontSizes { get; }

        // XAML content property
        private string _xamlDocument = string.Empty;

        // Current specific question ViewModel
        private BaseSpecificQuestionViewModel _currentContentVM;

        #region Configure
        public override string IconKind => "FileDocumentOutline";
        public override bool IsBlock => false;
        public override ObservableCollection<ExerciseContentItemViewModel>? Children { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; set; }
        public ICommand CancelAddQuestionCommand { get; set; }


        // Popup and formatting commands
        public ICommand OpenFontColorCommand { get; set; }
        public ICommand OpenMoreFontColorCommand { get; set; }
        public ICommand SelectMoreColorCommand { get; set; }
        public ICommand CancelMoreColorCommand { get; set; }
        public ICommand SelectColorCommand { get; set; }
        public ICommand SelectAlignCommand { get; set; }
        #endregion

        #region Properties
        public Question Model { get; }

        public QuestionType SelectedQuestionType
        {
            get => Model.Type;
            set
            {
                Model.Type = value;
                OnPropertyChanged();
                SwitchQuestionType();
            }
        }
        public ObservableCollection<SelectableEnumDisplay<QuestionType>> QuestionTypeOptions { get; }

        // Format and popup properties
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }
        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }
        public double SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }
        public TextAlignment SelectedAlignment
        {
            get => _selectedAlignment;
            set
            {
                _selectedAlignment = value;
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

        // Current specific question ViewModel
        public BaseSpecificQuestionViewModel CurrentContentVM
        {
            get => _currentContentVM;
            set
            {
                _currentContentVM = value;
                OnPropertyChanged(nameof(CurrentContentVM));
            }
        }

        #endregion

        public QuestionEditorViewModel(IAssetService assetService,
                                       IDialogService dialogService,
                                       IFormatConversionService formatService,
                                       Question model)
        {
            _assetService = assetService;
            _dialogService = dialogService;
            _formatService = formatService;
            Model = model;

            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32 };
            QuestionTypeOptions = new ObservableCollection<SelectableEnumDisplay<QuestionType>>(
                Enum.GetValues(typeof(QuestionType))
                    .Cast<QuestionType>()
                    .Select(qt => new SelectableEnumDisplay<QuestionType>(qt)));
            SwitchQuestionType();

            InitializeCommands();
        }

        /// <summary>
        /// Khởi tạo commands
        /// </summary>
        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand<object>(p => true, async p =>
            {
                // Debug: Save XAML content as base64 in Model.Content
                Model.AssetId = (await _assetService.GetOrAddTextAssetAsync(XamlDocument, AuthenticationCache.CurrentAuthentication!.User.Id)).Id;
                foreach (var answer in Model.AnswerOptions)
                {
                    var contentInXaml = _formatService.ConvertPlainTextToXaml(answer.Content);
                    answer.AssetId = (await _assetService.GetOrAddTextAssetAsync(contentInXaml, AuthenticationCache.CurrentAuthentication!.User.Id)).Id;
                }

                if (p is QuestionEditorWindow window)
                {
                    Model.Title = XamlDocument;
                    window.DialogResult = true;
                    window.Close();
                }
            });
            CancelAddQuestionCommand = new RelayCommand<object>(p => true, async p =>
            {
                if (p is QuestionEditorWindow window)
                {
                    window.DialogResult = false;
                    window.Close();
                }
            });

            OpenFontColorCommand = new RelayCommand<object>(p => true, p => TogglePopupOpen());
            OpenMoreFontColorCommand = new RelayCommand<object>(p => true, p => ShowMoreColorsWindow());
            SelectMoreColorCommand = new RelayCommand<object>(p => true, p => SelectMoreColor(p));
            CancelMoreColorCommand = new RelayCommand<object>(p => true, p => CancelMoreColor(p));
            SelectColorCommand = new RelayCommand<object>(p => true, p => SelectColor(p));
            SelectAlignCommand = new RelayCommand<object>(p => true, p => SelectAlignment(p));
        }

        private void SwitchQuestionType()
        {
            // Logic to switch question type and update Model accordingly
            switch (SelectedQuestionType)
            {
                case QuestionType.MultipleChoice:
                    CurrentContentVM = new MultipleChoiceViewModel(Model);
                    break;
                case QuestionType.Matching:
                    CurrentContentVM = new MultipleChoiceViewModel(Model);
                    break;
                case QuestionType.TrueFalse:
                    // Update Model for True/False
                    break;
                case QuestionType.FillInBlank:
                    // Update Model for Fill in the Blank
                    break;
                case QuestionType.Writing:
                    // Update Model for Writing
                    break;
                default:
                    break;
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
    }
}
