using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.Interfaces.Services;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels.Items;
using System.Collections.ObjectModel;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels
{
    public class ExerciseDetailViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IAssetService _assetService;
        private readonly IExerciseService _exerciseService;

        private Exercise _currExercise;

        #region Commands

        #endregion

        #region Properties
        public Exercise CurrExercise
        {
            get => _currExercise;
            set
            {
                _currExercise = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ExerciseContentItemViewModel> ContentItems { get; set; }
        #endregion

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ExerciseDetailViewModel(IServiceProvider serviceProvider,
                                       IAssetService assetService,
                                       IExerciseService exerciseService)
        {
            _serviceProvider = serviceProvider;
            _assetService = assetService;
            _exerciseService = exerciseService;

            ContentItems = new ObservableCollection<ExerciseContentItemViewModel>();
        }

        private void MapToDisplayItems(Exercise exercise)
        {
            ContentItems.Clear();

            int globalIndex = 1;

            // Mapping logic here
            var standaloneQuestions = exercise.Questions.Where(q => q.QuestionBlockId == null)
                                                        .Select(q => new { Type = "Question", Obj = (object)q, Order = q.OrderIndex });

            var questionBlocks = exercise.QuestionBlocks.Select(b => new { Type = "Block", Obj = (object)b, Order = b.OrderIndex });

            var combinedList = standaloneQuestions.Concat(questionBlocks)
                                                  .OrderBy(x => x.Order)
                                                  .ToList();

            foreach (var item in combinedList)
            {
                if (item.Type == "Question")
                {
                    var q = (Question)item.Obj;

                    var qVm = new DetailQuestionViewModel(_assetService, q, globalIndex);

                    ContentItems.Add(qVm);
                    globalIndex++;
                }
                else if (item.Type == "Block")
                {
                    var b = (QuestionBlock)item.Obj;

                    var bVM = new DetailBlockViewModel(_assetService, b, globalIndex);
                    ContentItems.Add(bVM);

                    globalIndex += b.Questions.Count;
                }
            }
        }

        public async Task LoadExerciseDetailAsync(long exerciseId)
        {
            _currExercise = await _exerciseService.GetExerciseByIdAsync(exerciseId) ?? new Exercise();
            OnPropertyChanged(nameof(CurrExercise));
            MapToDisplayItems(CurrExercise);
        }
    }
}
