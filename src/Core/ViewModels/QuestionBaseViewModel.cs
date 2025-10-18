using LaboratoryApp.src.Core.Models.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.ViewModels
{
    public abstract class QuestionBaseViewModel : BaseViewModel
    {
        public abstract Question BuildQuestion();
    }
}
