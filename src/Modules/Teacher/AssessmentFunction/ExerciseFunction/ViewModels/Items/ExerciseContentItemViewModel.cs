using LaboratoryApp.src.Core.ViewModels;
using System.Collections.ObjectModel;

namespace LaboratoryApp.src.Modules.Teacher.AssessmentFunction.ExerciseFunction.ViewModels.Items
{
    public abstract class ExerciseContentItemViewModel : BaseViewModel
    {
        public int OrderIndex { get; set; }
        public abstract string IconKind { get; }
        public abstract bool IsBlock { get; }

        public abstract ObservableCollection<ExerciseContentItemViewModel>? Children { get; set; }
    }
}
