using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class ReactionModel
    {
        public long ReactionId { get; set; }
        public IList<ReactionComponent> Reactants { get; set; } = new List<ReactionComponent>();
        public IList<ReactionComponent> Products { get; set; } = new List<ReactionComponent>();
        public double? YieldPercent { get; set; }     // Reaction yield
        public string Conditions { get; set; }        // e.g., temperature, pressure, catalyst
        public DateTime? ReactionDate { get; set; }
        public string Notes { get; set; }
    }
}
