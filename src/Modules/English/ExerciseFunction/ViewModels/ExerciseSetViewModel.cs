using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.English.ExerciseFunction.Enums;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.English.ExerciseFunction.Views;
using LaboratoryApp.src.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.English.ExerciseFunction.ViewModels
{
    public class ExerciseSetViewModel : BaseViewModel, IAsyncInitializable
    {
        private readonly IServiceProvider _serviceProvider;

        private string _title;
        private string _description;
        private string _code;
        private string? _password;
        private bool _isPublic;
        private bool _requireLogin;

        #region Properties
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public bool IsPublic
        {
            get => _isPublic;
            set
            {
                _isPublic = value;
                OnPropertyChanged();
            }
        }
        public bool RequireLogin
        {
            get => _requireLogin;
            set
            {
                _requireLogin = value;
                OnPropertyChanged();
            }
        }
        public ExerciseSetType SelectedExerciseSetType { get; set; }
        public DifficultyLevel SelectedDifficultyLevel { get; set; }
        public ObservableCollection<SelectableEnumDisplay<ExerciseSetType>> ExerciseTypeOptions { get; }
        public ObservableCollection<SelectableEnumDisplay<DifficultyLevel>> DifficultyLevelOptions { get; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ExerciseSetViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _title = string.Empty;
            _description = string.Empty;
            _code = string.Empty;
            _password = null;
            _isPublic = false;
            _requireLogin = false;

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
