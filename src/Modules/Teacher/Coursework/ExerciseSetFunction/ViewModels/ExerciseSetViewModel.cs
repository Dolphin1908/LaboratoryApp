using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.Domain.Helpers;
using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.ViewModels
{
    public class ExerciseSetViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IExerciseSetService _exerciseSetService;

        private ExerciseSet _exerciseSet;

        #region Properties
        public ExerciseSet ExerciseSet
        {
            get => _exerciseSet;
            set
            {
                _exerciseSet = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SelectableEnumDisplay<DifficultyLevel>> DifficultyLevelOptions { get; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ExerciseSetViewModel(IServiceProvider serviceProvider,
                                    IExerciseSetService exerciseSetService)
        {
            _serviceProvider = serviceProvider;
            _exerciseSetService = exerciseSetService;

            _exerciseSet = new ExerciseSet
            {
                Title = string.Empty,
                Description = string.Empty,
            };

            DifficultyLevelOptions = new ObservableCollection<SelectableEnumDisplay<DifficultyLevel>>(
                Enum.GetValues(typeof(DifficultyLevel))
                    .Cast<DifficultyLevel>()
                    .Select(e => new SelectableEnumDisplay<DifficultyLevel>(e))
            );

            // Khởi tạo lệnh ở đây
            #region Commands
            SaveCommand = new RelayCommand<object>(p => true, (p) =>
            {
                _exerciseSetService.SaveNewExerciseSet(ExerciseSet);

                if (p is AddExerciseSetWindow window)
                {
                    window.Close();
                }
            });
            #endregion
        }
    }
}
