using LaboratoryApp.src.Core.Models.Chemistry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Chemistry.PeriodicFunction
{
    public interface IPeriodicProvider
    {
        List<Element> GetAllElements();
    }
}
