using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class ReactionCondition
    {
        public long Id { get; set; }

        public string Temperature { get; set; } = string.Empty;

        public string Pressure { get; set; } = string.Empty;

        public string? Catalyst { get; set; }

        public string? Solvent { get; set; }

        public string? PH { get; set; }

        public List<string>? OtherConditions { get; set; }
    }
}
