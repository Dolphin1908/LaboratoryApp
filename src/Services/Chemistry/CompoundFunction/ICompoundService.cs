using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Helpers;

using LaboratoryApp.src.Core.Models.Chemistry;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;

using LaboratoryApp.src.Modules.Teacher.Chemistry.CompoundFunction.ViewModels;

namespace LaboratoryApp.src.Services.Chemistry.CompoundFunction
{
    public interface ICompoundService
    {
        IEnumerable<Compound> GetSuggestions(string searchText, int limit = 10);
        void SaveCompound(Compound compound);
    }
}