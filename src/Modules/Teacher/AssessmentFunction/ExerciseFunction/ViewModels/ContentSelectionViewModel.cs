using LaboratoryApp.Domain.Enums.Content;
using LaboratoryApp.src.Core.ViewModels;
using LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.Views;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels
{
    public class ContentSelectionViewModel : BaseViewModel
    {
        public ContentSelectionResult Result { get; set; }

        public ICommand SelectSingleQuestionCommand { get; set; }
        public ICommand SelectQuestionBlockCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ContentSelectionViewModel()
        {
            SelectSingleQuestionCommand = new RelayCommand<object>(p => true, p =>
            {
                Result = ContentSelectionResult.SingleQuestion;
                CloseWindow(p);
            });
            SelectQuestionBlockCommand = new RelayCommand<object>(p => true, p =>
            {
                Result = ContentSelectionResult.QuestionBlock;
                CloseWindow(p);
            });
            CancelCommand = new RelayCommand<object>(p => true, p =>
            {
                Result = ContentSelectionResult.Cancel;
                CloseWindow(p);
            });
        }

        private void CloseWindow(object p)
        {
            // Implement window close logic if needed
            if (p is ContentSelectionWindow window)
            {
                window.Close();
            }
        }
    }
}
