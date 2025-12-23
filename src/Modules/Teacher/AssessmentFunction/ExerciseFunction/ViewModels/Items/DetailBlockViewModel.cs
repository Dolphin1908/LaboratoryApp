using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Interfaces.Services;
using System.Collections.ObjectModel;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels.Items
{
    public class DetailBlockViewModel : ExerciseContentItemViewModel
    {
        private readonly IAssetService _assetService;

        #region Configure
        public override string IconKind => "FileDocumentMultipleOutline";
        public override bool IsBlock => true;
        public override ObservableCollection<ExerciseContentItemViewModel>? Children { get; set; }
        #endregion

        #region Properties
        public string DisplayIndex { get; set; } = string.Empty;
        public QuestionBlock Model { get; set; }
        #endregion

        public DetailBlockViewModel(IAssetService assetService, QuestionBlock model, int startIndex)
        {
            _assetService = assetService;
            LoadData(model, startIndex);
        }

        private async void LoadData(QuestionBlock model, int startIndex)
        {
            Model = model;
            Model.Content = await _assetService.GetTextContentAsync(model.AssetId);
            Children = new ObservableCollection<ExerciseContentItemViewModel>();

            var sortedQuestions = model.Questions.OrderBy(q => q.OrderIndex).ToList();

            int localIndex = startIndex;

            foreach (var question in sortedQuestions)
            {
                var qVm = new DetailQuestionViewModel(_assetService, question, localIndex);
                Children.Add(qVm);
                localIndex++;
            }
        }
    }
}
