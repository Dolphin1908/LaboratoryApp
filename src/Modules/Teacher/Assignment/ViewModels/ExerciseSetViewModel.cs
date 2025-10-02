using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using LaboratoryApp.src.Constants;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Assignment;
using LaboratoryApp.src.Core.Models.Assignment.Enums;
using LaboratoryApp.src.Core.ViewModels;

using LaboratoryApp.src.Modules.Teacher.Assignment.Views;

using LaboratoryApp.src.Services.Assignment;
using LaboratoryApp.src.Services.Counter;

using LaboratoryApp.src.Shared.Interface;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.ViewModels
{
    public class ExerciseSetViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAssignmentService _assignmentService;
        private readonly ICounterService _counterService;

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
        public ObservableCollection<SelectableEnumDisplay<ExerciseSetType>> ExerciseTypeOptions { get; }
        public ObservableCollection<SelectableEnumDisplay<DifficultyLevel>> DifficultyLevelOptions { get; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ExerciseSetViewModel(IServiceProvider serviceProvider,
                                    IAssignmentService assignmentService,
                                    ICounterService counterService)
        {
            _serviceProvider = serviceProvider;
            _assignmentService = assignmentService;
            _counterService = counterService;

            _exerciseSet = new ExerciseSet
            {
                Title = string.Empty,
                Description = string.Empty,
                Code = string.Empty,
                Password = null,
                IsPublic = false,
                RequireLogin = false
            };

            ExerciseTypeOptions = new ObservableCollection<SelectableEnumDisplay<ExerciseSetType>>(
                Enum.GetValues(typeof(ExerciseSetType))
                    .Cast<ExerciseSetType>()
                    .Select(e => new SelectableEnumDisplay<ExerciseSetType>(e))
            );
            DifficultyLevelOptions = new ObservableCollection<SelectableEnumDisplay<DifficultyLevel>>(
                Enum.GetValues(typeof(DifficultyLevel))
                    .Cast<DifficultyLevel>()
                    .Select(e => new SelectableEnumDisplay<DifficultyLevel>(e))
            );

            // Khởi tạo lệnh ở đây
            #region Commands
            SaveCommand = new RelayCommand<object>(p => true, (p) =>
            {
                ExerciseSet.Id = _counterService.GetNextId(CollectionName.ExerciseSets);

                if (string.IsNullOrWhiteSpace(ExerciseSet.Title))
                {
                    MessageBox.Show("Vui lòng nhập tên bộ!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ExerciseSet.Password = string.IsNullOrEmpty(ExerciseSet.Password) ? null : SecureConfigHelper.Encrypt(ExerciseSet.Password);
                ExerciseSet.Code = GenerateUniqueCode();

                // Lưu bộ bài tập
                try
                { 
                    _assignmentService.AddExerciseSet(ExerciseSet); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu bộ bài tập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (p is AddExerciseSetWindow window)
                {
                    window.Close();
                }
            });
            #endregion
        }

        private string GenerateUniqueCode()
        {
            string code = string.Empty;

            return code;
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
