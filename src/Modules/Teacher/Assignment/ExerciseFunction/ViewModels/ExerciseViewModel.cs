using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Helpers;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Services.Assignment.ExerciseFunction;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels
{
    public class ExerciseViewModel : BaseViewModel
    {
        private readonly IExerciseService _exerciseService;
        private readonly ExerciseSet _currSet;

        private Exercise _exercise;

        #region Commands
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Properties
        public Exercise Exercise
        {
            get => _exercise;
            set
            {
                _exercise = value;
                OnPropertyChanged(nameof(Exercise));
            }
        }
        public ObservableCollection<SelectableEnumDisplay<ExerciseType>> ExerciseTypeOptions { get; set; }
        public ObservableCollection<SelectableEnumDisplay<DifficultyLevel>> DifficultyLevelOptions { get; set; }
        #endregion

        public ExerciseViewModel(IExerciseService exerciseService,
                                 ExerciseSet currSet)
        {
            _exerciseService = exerciseService;
            _currSet = currSet;

            Exercise = new Exercise
            {
                Title = string.Empty,
                Description = string.Empty
            };
            ExerciseTypeOptions = new ObservableCollection<SelectableEnumDisplay<ExerciseType>>(
                Enum.GetValues(typeof(ExerciseType))
                    .Cast<ExerciseType>()
                    .Select(e => new SelectableEnumDisplay<ExerciseType>(e))
            );
            DifficultyLevelOptions = new ObservableCollection<SelectableEnumDisplay<DifficultyLevel>>(
                Enum.GetValues(typeof(DifficultyLevel))
                    .Cast<DifficultyLevel>()
                    .Select(d => new SelectableEnumDisplay<DifficultyLevel>(d))
            );

            #region Commands
            SaveCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Implement save logic here
                _exerciseService.SaveNewExercise(_currSet, Exercise);

                if (p is Window win)
                {
                    win.Close();
                }
            });
            #endregion
        }
    }
}
