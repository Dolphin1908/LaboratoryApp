using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class ReactionCondition
    {
        public string? Temperature { get; set; } // Nhiệt độ

        public string? Pressure { get; set; } // Áp suất

        public string? Catalyst { get; set; } // Chất xúc tác

        public string? Solvent { get; set; } // Dung môi

        public string? PH { get; set; } // Độ pH

        public List<ReactionOtherCondition>? OtherConditions { get; set; }
    }

    public class ReactionOtherCondition
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
