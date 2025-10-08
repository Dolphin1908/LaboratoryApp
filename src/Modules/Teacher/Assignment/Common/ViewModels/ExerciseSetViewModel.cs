using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.src.Constants;
using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Assignment.Enums;
using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Teacher.Assignment.Common.Views;

using LaboratoryApp.src.Services.Assignment;
using LaboratoryApp.src.Services.Counter;

using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.Common.ViewModels
{
    public class ExerciseSetViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAssignmentService _assignmentService;

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
                                    IAssignmentService assignmentService)
        {
            _serviceProvider = serviceProvider;
            _assignmentService = assignmentService;

            _exerciseSet = new ExerciseSet
            {
                Title = string.Empty,
                Description = string.Empty,
                Code = string.Empty,
                Password = null
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
                _assignmentService.SaveNewExerciseSet(ExerciseSet);

                if (p is AddExerciseSetWindow window)
                {
                    window.Close();
                }
            });
            #endregion
        }

        /// <summary>
        /// Khởi tạo dữ liệu bất đồng bộ
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                // Khởi tạo dữ liệu bất đồng bộ ở đây
            }, cancellationToken);
        }
    }
}
