using LaboratoryApp.Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Modules.Teacher.Coursework.QuestionFunction.ViewModels
{
    public class OptionViewModel
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;

        public OptionViewModel()
        {

        }

        public AnswerOption BuildOption ()
        {
            return new AnswerOption
            {
                OptionText = Text ?? string.Empty,
                IsCorrect = IsCorrect
            };
        }
    }
}
