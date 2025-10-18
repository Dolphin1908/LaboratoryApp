using LaboratoryApp.src.Core.Models.Chemistry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Data.Providers.Chemistry.CompoundFunction
{
    public interface ICompoundProvider
    {
        List<Compound> GetAllCompounds();
        void AddCompound(Compound compound);
        void UpdateCompound(Compound compound);
        void DeleteCompound(Compound compound);
    }
}
