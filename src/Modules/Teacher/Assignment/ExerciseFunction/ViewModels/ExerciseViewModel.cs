using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Core.Models.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LaboratoryApp.src.Services.Assignment;
using System.Windows;
using System.Collections.ObjectModel;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Assignment.Enums;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels
{
    public class ExerciseViewModel : BaseViewModel
    {
        private readonly IAssignmentService _assignmentService;
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

        public ExerciseViewModel(IAssignmentService assignmentService,
                                 ExerciseSet currSet)
        {
            _assignmentService = assignmentService;
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
                _assignmentService.SaveNewExercise(_currSet, Exercise);

                if (p is Window win)
                {
                    win.Close();
                }
            });
            #endregion
        }
    }
}
