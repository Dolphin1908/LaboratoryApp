using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class CompoundModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Formula { get; set; }
        public double MolecularMass { get; set; }
        public string Phase { get; set; }          // e.g., Solid, Liquid, Gas
        public double? Density { get; set; }       // Optional: g/cm³
        public string CASNumber { get; set; }      // Optional chemical identifier
        public List<CompoundElement> Composition { get; set; } // List of elements and their quantities
    }
}
