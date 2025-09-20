using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using LaboratoryApp.src.Core.Caches;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class ReactionComponent
    {
        public SubstanceKind Kind { get; set; } // Element or Compound

        public long? ElementId { get; set; }

        public long? CompoundId { get; set; }

        public decimal Coefficient { get; set; } // Hệ số tỉ lệ

        [NotMapped]
        public string Formula => ChemistryDataCache.AllElements.FirstOrDefault(e => e.Id == ElementId)?.Formula ?? ChemistryDataCache.AllCompounds.FirstOrDefault(c => c.Id == CompoundId)?.Formula ?? string.Empty;

        [NotMapped]
        public string DisplayCoefficient
        {
            get
            {
                if (Coefficient == 1)
                    return string.Empty; // Do not display coefficient if it is 1
                
                if (Coefficient % 1 == 0)
                    return ((int)Coefficient).ToString(); // Display as integer if it is a whole number

                return Coefficient.ToString("0.##"); // Display up to 2 decimal places
            }
        }

        [NotMapped]
        public string Display => string.IsNullOrEmpty(DisplayCoefficient) ? Formula : $"{DisplayCoefficient} {Formula}";
    }
}