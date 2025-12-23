using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Interfaces.Services.Common;
using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Interfaces;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels.Items;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.Views;
using LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels
{
    public class ContentViewModel : BaseViewModel, IPageLifecycle
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAssetService _assetService;
        private readonly ICounterService _counterService;
        private readonly IDialogService _dialogService;
        private readonly IFormatConversionService _formatConversionService;

        private Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel> _questionEditorVmFactory;
        private Func<IServiceProvider, IAssetService, IDialogService, IFormatConversionService, QuestionBlock, Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel>, QuestionBlockEditorViewModel> _questionBlockEditorVmFactory;

        private List<ExerciseContentItemViewModel> _newlyCreatedItems = new List<ExerciseContentItemViewModel>();
        private bool _isSaved = false;

        #region Commands
        public ICommand CreateNewQuestionCommand { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<ExerciseContentItemViewModel> ExerciseItems { get; set; }
        public Exercise Result { get; set; }
        #endregion

        public ContentViewModel(IServiceProvider serviceProvider,
                                IAssetService assetService,
                                ICounterService counterService,
                                IDialogService dialogService,
                                IFormatConversionService formatConversionService,
                                Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel> questionEditorVmFactory,
                                Func<IServiceProvider, IAssetService, IDialogService, IFormatConversionService, QuestionBlock, Func<IAssetService, IDialogService, IFormatConversionService, Question, QuestionEditorViewModel>, QuestionBlockEditorViewModel> questionBlockEditorVmFactory)
        {
            _serviceProvider = serviceProvider;
            _assetService = assetService;
            _counterService = counterService;
            _dialogService = dialogService;
            _formatConversionService = formatConversionService;
            _questionEditorVmFactory = questionEditorVmFactory;
            _questionBlockEditorVmFactory = questionBlockEditorVmFactory;

            ExerciseItems = new ObservableCollection<ExerciseContentItemViewModel>();

            CreateNewQuestionCommand = new RelayCommand<object>(p => true, p =>
            {
                var selectionWindow = _serviceProvider.GetRequiredService<ContentSelectionWindow>();
                _dialogService.ShowDialogCenterOwner(selectionWindow);

                var selectionVM = selectionWindow.DataContext as ContentSelectionViewModel;
                if (selectionVM == null || selectionVM.Result == ContentSelectionResult.Cancel)
                {
                    return;
                }
                else if (selectionVM.Result == ContentSelectionResult.SingleQuestion)
                {
                    // Open Create New Single Question Window
                    if (_questionEditorVmFactory != null)
                    {
                        var newQuestion = new Question
                        {
                            Type = QuestionType.MultipleChoice
                        };

                        var vm = _questionEditorVmFactory(_assetService, _dialogService, _formatConversionService, newQuestion);
                        var window = _serviceProvider.GetRequiredService<QuestionEditorWindow>();
                        window.DataContext = vm;
                        if (_dialogService.ShowDialogCenterOwner(window) == true)
                        {
                            ExerciseItems.Add(vm);
                            _newlyCreatedItems.Add(vm);
                        }
                    }
                }
                else if (selectionVM.Result == ContentSelectionResult.QuestionBlock)
                {
                    // Open Create New Question Block Window
                    if (_questionBlockEditorVmFactory != null)
                    {
                        var vm = _questionBlockEditorVmFactory(_serviceProvider, _assetService, _dialogService, _formatConversionService, new QuestionBlock(), _questionEditorVmFactory);
                        var window = _serviceProvider.GetRequiredService<QuestionBlockEditorWindow>();
                        window.DataContext = vm;
                        if (_dialogService.ShowDialogCenterOwner(window) == true)
                        {
                            ExerciseItems.Add(vm);
                            _newlyCreatedItems.Add(vm);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Thực thi xóa một mục trong danh sách ExerciseItems
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task ExecuteDeleteItem(ExerciseContentItemViewModel item)
        {
            var confirm = _dialogService.ShowMessage("Bạn có chắc chắn muốn xóa mục này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.No) return;

            await DeleteAssetRecursive(item); // Xóa tài nguyên liên quan đến mục

            ExerciseItems.Remove(item); // Xóa mục khỏi danh sách
        }

        /// <summary>
        /// Thu hồi tài nguyên liên quan đến mục một cách đệ quy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task DeleteAssetRecursive(ExerciseContentItemViewModel item)
        {
            if (item is QuestionEditorViewModel qVm)
            {
                if (qVm.Model.AssetId > 0)
                {
                    await _assetService.ReleaseAssetAsync(qVm.Model.AssetId);

                    foreach (var option in qVm.Model.AnswerOptions)
                    {
                        if (option.AssetId > 0)
                        {
                            await _assetService.ReleaseAssetAsync(option.AssetId);
                        }
                    }
                }
            }
            else if (item is QuestionBlockEditorViewModel qbVm)
            {
                if (qbVm.Model.AssetId > 0)
                {
                    await _assetService.ReleaseAssetAsync(qbVm.Model.AssetId);
                }

                if (qbVm.Children != null)
                {
                    foreach (var child in qbVm.Children)
                    {
                        await DeleteAssetRecursive(child);
                    }
                }
            }
        }

        private void ExecuteEditItem(ExerciseContentItemViewModel item)
        {
            if (item is QuestionEditorViewModel qVm)
            {
                var window = _serviceProvider.GetRequiredService<QuestionEditorWindow>();
                window.DataContext = qVm;
                _dialogService.ShowDialogCenterOwner(window);
            }
            else if (item is QuestionBlockEditorViewModel qbVm)
            {
                var window = _serviceProvider.GetRequiredService<QuestionBlockEditorWindow>();
                window.DataContext = qbVm;
                _dialogService.ShowDialogCenterOwner(window);
            }
        }

        public async Task OnCleanupAsync()
        {
            if (_isSaved) return;
            // Thu hồi tài nguyên của các mục mới được tạo
            foreach (var item in _newlyCreatedItems)
            {
                await DeleteAssetRecursive(item);
            }
            _newlyCreatedItems.Clear();
        }

        public async Task OnSaveAsync()
        {
            _isSaved = true;

            Result = new Exercise
            {
                Questions = new List<Question>(),
                QuestionBlocks = new List<QuestionBlock>()
            };
            int currentIndex = 0;

            foreach (var item in ExerciseItems)
            {
                if (item is QuestionEditorViewModel qVm)
                {
                    var question = qVm.Model;

                    question.Id = _counterService.GetNextId(CollectionName.Questions);

                    question.RootExercise = Result;
                    question.OrderIndex = currentIndex;

                    question.QuestionBlock = null;
                    question.QuestionBlockId = null;

                    foreach (var opt in question.AnswerOptions)
                    {
                        opt.Id = _counterService.GetNextId(CollectionName.AnswerOptions);

                        opt.Question = question;
                        opt.QuestionId = question.Id;
                    }

                    Result.Questions.Add(question);
                }
                else if (item is QuestionBlockEditorViewModel qbVm)
                {
                    var block = qbVm.Model;

                    block.Id = _counterService.GetNextId(CollectionName.QuestionBlocks);
                    block.Title = _formatConversionService.ConvertXamlToPlainText(block.Title);

                    block.RootExercise = Result;
                    block.OrderIndex = currentIndex;

                    block.Questions.Clear();

                    if (qbVm.Children != null)
                    {
                        int childIndex = 0;
                        foreach (var child in qbVm.Children)
                        {
                            if (child is QuestionEditorViewModel childQVm)
                            {
                                var question = childQVm.Model;

                                question.Id = _counterService.GetNextId(CollectionName.Questions);

                                question.RootExercise = Result;
                                question.OrderIndex = childIndex;

                                question.QuestionBlock = block;
                                question.QuestionBlockId = block.Id;

                                foreach (var opt in question.AnswerOptions)
                                {
                                    opt.Id = _counterService.GetNextId(CollectionName.AnswerOptions);

                                    opt.Question = question;
                                    opt.QuestionId = question.Id;
                                }
                                block.Questions.Add(question);
                            }
                            childIndex++;
                        }
                    }
                    Result.QuestionBlocks.Add(block);
                }
                currentIndex++;
            }
            // Xóa danh sách các mục mới được tạo để không thu hồi tài nguyên trong OnCleanupAsync
            _newlyCreatedItems.Clear();
        }
    }
}
