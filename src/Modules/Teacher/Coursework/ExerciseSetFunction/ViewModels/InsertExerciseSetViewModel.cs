using LaboratoryApp.Domain.Interfaces.Services.Content;
using LaboratoryApp.src.Core.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.Coursework.ExerciseSetFunction.ViewModels
{
    public class InsertExerciseSetViewModel : BaseViewModel
    {
        private readonly IExerciseSetService _exerciseSetService;

        private string _code = string.Empty;
        private string _password = string.Empty;

        #region Commands
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Properties
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion

        public InsertExerciseSetViewModel(IExerciseSetService exerciseSetService)
        {
            _exerciseSetService = exerciseSetService;

            #region Commands
            SaveCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var result = _exerciseSetService.InsertNewExerciseSet(Code, Password);

                if (p is Window win && result == true)
                {
                    win.Close();
                }
            });
            #endregion
        }
    }
}
