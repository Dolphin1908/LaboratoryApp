using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        private bool _shuffleQuestions = false;
        private bool _shuffleAnswers = false;
        private bool _showResultImmediately = false;
        private bool _showCorrectAnswers = false;
        private bool _allowLateSubmission = false;
        private int _lateSubmissionPenalty = 10;
        private int _maxAttempts = 1;

        public bool ShuffleQuestions
        {
            get => _shuffleQuestions;
            set
            {
                _shuffleQuestions = value;
                OnPropertyChanged();
            }
        }
        public bool ShuffleAnswers
        {
            get => _shuffleAnswers;
            set
            {
                _shuffleAnswers = value;
                OnPropertyChanged();
            }
        }
        public bool ShowResultImmediately
        {
            get => _showResultImmediately;
            set
            {
                _showResultImmediately = value;
                OnPropertyChanged();
            }
        }
        public bool ShowCorrectAnswers
        {
            get => _showCorrectAnswers;
            set
            {
                _showCorrectAnswers = value;
                OnPropertyChanged();
            }
        }
        public bool AllowLateSubmission
        {
            get => _allowLateSubmission;
            set
            {
                _allowLateSubmission = value;
                OnPropertyChanged();
            }
        }
        public int LateSubmissionPenalty
        {
            get => _lateSubmissionPenalty;
            set
            {
                _lateSubmissionPenalty = value;
                OnPropertyChanged();
            }
        }
        public int MaxAttempts
        {
            get => _maxAttempts;
            set
            {
                _maxAttempts = value;
                OnPropertyChanged();
            }
        }

        public SettingViewModel() { }
    }
}
