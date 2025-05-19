using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryApp.Models.Chemistry.Enums;

namespace LaboratoryApp.Models.Chemistry
{
    public class ReactionComponent
    {
        public SubstanceKind Kind { get; set; } // Element or Compound

        public Element? Element { get; set; }

        public Compound? Compound { get; set; }

        public string StoichiometricCoefficient { get; set; } = string.Empty; // Hệ số tỉ lệ
    }
}
