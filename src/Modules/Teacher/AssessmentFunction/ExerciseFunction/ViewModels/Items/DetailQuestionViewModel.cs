using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Interfaces.Services;
using System.Collections.ObjectModel;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels.Items
{
    public class DetailQuestionViewModel : ExerciseContentItemViewModel
    {
        private readonly IAssetService _assetService;

        private string _displayIndex = string.Empty;

        #region Configure
        public override string IconKind => "FileDocumentMultipleOutline";
        public override bool IsBlock => true;
        public override ObservableCollection<ExerciseContentItemViewModel>? Children { get; set; }
        #endregion

        #region Properties
        public string DisplayIndex
        {
            get => _displayIndex;
            set
            {
                _displayIndex = value;
                OnPropertyChanged();
            }
        }
        public Question Model { get; set; }
        #endregion

        public DetailQuestionViewModel(IAssetService assetService, Question model, int questionIndex)
        {
            _assetService = assetService;
            LoadData(model, questionIndex);
        }

        private async void LoadData(Question model, int questionIndex)
        {
            Model = model;
            Model.Title = await _assetService.GetTextContentAsync(Model.AssetId);
            DisplayIndex = $"Câu {questionIndex}";
            OrderIndex = questionIndex;
        }
    }
}
