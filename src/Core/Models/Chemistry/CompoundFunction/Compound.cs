using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class Compound
    {
        public long Id { get; set; }

        public long OwnerId { get; set; } // User who created or owns the compound

        public string Author { get; set; } = string.Empty; // Author's name

        public string Name { get; set; } = string.Empty;

        public string Formula { get; set; } = string.Empty;

        public double MolecularMass { get; set; }

        public string CompoundColor { get; set; } = string.Empty;

        public string CompoundColorCode { get; set; } = string.Empty; // Hex color code

        public List<ChemicalPhase> Phases { get; set; } = new List<ChemicalPhase>();

        public double? Density { get; set; } // Density in g/cm³

        public string CASNumber { get; set; } = string.Empty; // Chemical Abstracts Service number

        public List<CompoundType> CompoundTypes { get; set; } = new List<CompoundType>(); // List of compound types

        public List<CompoundComponent> Composition { get; set; } = new List<CompoundComponent>(); // List of elements in the compound

        public List<ChemicalProperty> ChemicalProperties { get; set; } = new List<ChemicalProperty>(); // List of chemical properties

        public List<PhysicalProperty> PhysicalProperties { get; set; } = new List<PhysicalProperty>(); // List of physical properties

        public List<CompoundNote> Notes { get; set; } = new List<CompoundNote>(); // List of notes

        public string PhaseDescription =>
            Phases == null || Phases.Count == 0
              ? string.Empty
              : string.Join(", ", Phases.Select(p => p.GetDisplayName()));

        public string TypeDescription =>
            CompoundTypes == null || CompoundTypes.Count == 0
              ? string.Empty
              : string.Join(", ", CompoundTypes.Select(t => t.GetDisplayName()));
    }
}
