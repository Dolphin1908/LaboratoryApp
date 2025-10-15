using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Caches;

using LaboratoryApp.src.Core.Models.Chemistry.Enums;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class ReactionComponent
    {
        public SubstanceKind Kind { get; set; } // Element or Compound

        public long? ElementId { get; set; }

        public long? CompoundId { get; set; }

        public string Coefficient { get; set; } // Hệ số tỉ lệ

        [NotMapped]
        public string Formula { get; set; } = string.Empty; // Công thức

        [NotMapped]
        public string DisplayCoefficient { get; set; } = string.Empty; // Hệ số hiển thị (bỏ 1)

        [NotMapped]
        public string Display { get; set; } = string.Empty; // Hiển thị (Hệ số + Công thức)
    }
}