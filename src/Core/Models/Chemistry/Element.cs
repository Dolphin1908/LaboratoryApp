using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class Element
    {
        public long Id { get; set; }

        public string AtomicNumber { get; set; } = string.Empty; // Atomic number of the element

        public string Formula { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        
        public double AtomicMass { get; set; }
        
        public string Color { get; set; } = string.Empty; // Color of the element in the periodic table
        
        public string CPKColor { get; set; } = string.Empty; // Real color of the element

        // Raw storage for real-world colors (comma-separated)
        public string RealColorRaw { get; set; } = string.Empty;

        // Exposed as a list of color codes
        [NotMapped]
        public List<string> RealColors
        {
            get => string.IsNullOrWhiteSpace(RealColorRaw)
                ? new List<string>()
                : RealColorRaw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(c => c.Trim())
                               .ToList();
            set => RealColorRaw = value != null
                ? string.Join(',', value)
                : string.Empty;
        }

        public string ElectronConfiguration { get; set; } = string.Empty;
        
        public double? Electronegativity { get; set; }
        
        public int AtomicRadius { get; set; } // Picometer
        
        public double? IonizationEnergy { get; set; }
        
        public double? ElectronAffinity { get; set; }
        
        public string OxidationStates { get; set; } = string.Empty;
        
        public string Phase { get; set; } = string.Empty;
        
        public double? MeltingPoint { get; set; }
        
        public double? BoilingPoint { get; set; }
        
        public double? Density { get; set; }
        
        public string Category { get; set; } = string.Empty;
        
        public string DiscoveryYear { get; set; } = string.Empty;

        // Periodic table position
        public int Row { get; set; }
        
        public int Column { get; set; }
    }
}
