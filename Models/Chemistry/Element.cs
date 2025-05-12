using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class Element
    {
        public long Id { get; set; }

        public string AtomicNumber { get; set; } = String.Empty; // Atomic number of the element

        public string Symbol { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;
        
        public double AtomicMass { get; set; }
        
        public string Color { get; set; } = String.Empty; // Color of the element in the periodic table
        
        public string ElementColor { get; set; } = String.Empty; // Real color of the element
        
        public string ElectronConfiguration { get; set; } = String.Empty;
        
        public double? Electronegativity { get; set; }
        
        public int AtomicRadius { get; set; } // Picometer
        
        public double? IonizationEnergy { get; set; }
        
        public double? ElectronAffinity { get; set; }
        
        public string OxidationStates { get; set; } = String.Empty;
        
        public string Phase { get; set; } = String.Empty;
        
        public double? MeltingPoint { get; set; }
        
        public double? BoilingPoint { get; set; }
        
        public double? Density { get; set; }
        
        public string Category { get; set; } = String.Empty;
        
        public string DiscoveryYear { get; set; } = String.Empty;

        // Periodic table position
        public int Row { get; set; }
        
        public int Column { get; set; }
    }
}
