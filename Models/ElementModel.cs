using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models
{
    public class ElementModel
    {
        public int AtomicNumber { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double AtomicMass { get; set; }
        public string Color { get; set; }
        public string ElectronConfiguration { get; set; }
        public double? Electronegativity { get; set; }
        public int AtomicRadius { get; set; }
        public double IonizationEnergy { get; set; }
        public double? ElectronAffinity { get; set; }
        public string OxidationStates { get; set; }
        public string Phase { get; set; }
        public double MeltingPoint { get; set; }
        public double BoilingPoint { get; set; }
        public double Density { get; set; }
        public string Category { get; set; }
        public string DiscoveryYear { get; set; }
    }
}
