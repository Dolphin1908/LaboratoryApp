using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels
{
    public class InformationViewModel : BaseViewModel
    {
        private string _title = string.Empty;
        private string? _description;
        private int _durationInMinutes = 0;
        private string _passScore = "5.0";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public int DurationInMinutes
        {
            get => _durationInMinutes;
            set
            {
                _durationInMinutes = value;
                OnPropertyChanged();
            }
        }
        public string PassScore
        {
            get => _passScore;
            set
            {
                _passScore = value;
                OnPropertyChanged();
            }
        }

        public InformationViewModel() { }
    }
}
