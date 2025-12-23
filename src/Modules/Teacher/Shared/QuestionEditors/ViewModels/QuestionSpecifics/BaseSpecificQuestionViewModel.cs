using LaboratoryApp.Domain.Models.Content;
using LaboratoryApp.src.Core.ViewModels;

namespace LaboratoryApp.src.Modules.Teacher.Shared.QuestionEditors.ViewModels.QuestionSpecifics
{
    public abstract class BaseSpecificQuestionViewModel : BaseViewModel
    {
        public Question Model { get; }

        protected BaseSpecificQuestionViewModel(Question model)
        {
            Model = model;
        }
    }
}
