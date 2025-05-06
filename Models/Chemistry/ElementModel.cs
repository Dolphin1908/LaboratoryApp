using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    #region Element
    public class ElementModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string AtomicNumber { get; set; }

        [Required, StringLength(3)]
        public string Symbol { get; set; } = String.Empty;

        [Required, StringLength(50)]
        public string Name { get; set; } = String.Empty;
        public double AtomicMass { get; set; }
        public string Color { get; set; } = String.Empty;
        public string ElectronConfiguration { get; set; } = String.Empty;
        public double? Electronegativity { get; set; }

        // Picometer
        public int AtomicRadius { get; set; }
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
        public ICollection<CompoundElement>? CompoundElements { get; set; } // List of compounds containing this element
    }
    #endregion
}