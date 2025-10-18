using LaboratoryApp.src.Core.Caches;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Services.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Assignment.Common.ViewModels
{
    public class InsertExerciseSetViewModel : BaseViewModel
    {
        private readonly IAssignmentService _assignmentService;

        private string _code;
        private string _password;

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

        public InsertExerciseSetViewModel(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;

            #region Commands
            SaveCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var result = _assignmentService.InsertNewExerciseSet(Code, Password);

                if(p is Window win && result == true)
                {
                    win.Close();
                }    
            });
            #endregion
        }
    }
}
