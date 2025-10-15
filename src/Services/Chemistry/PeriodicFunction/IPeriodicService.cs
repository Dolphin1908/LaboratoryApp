using LaboratoryApp.src.Core.Models.Chemistry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Services.Chemistry.PeriodicFunction
{
    public interface IPeriodicService
    {
        List<Element> LoadAllElements();
    }
}
