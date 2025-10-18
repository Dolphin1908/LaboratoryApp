using LaboratoryApp.src.Core.Models.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Modules.Teacher.Assignment.ExerciseFunction.ViewModels
{
    public class OptionViewModel
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;

        public OptionViewModel()
        {

        }

        public Option BuildOption ()
        {
            return new Option
            {
                Text = Text ?? string.Empty,
                IsCorrect = IsCorrect
            };
        }
    }
}
