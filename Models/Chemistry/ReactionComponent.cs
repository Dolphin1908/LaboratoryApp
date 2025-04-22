using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class ReactionComponent
    {
        public CompoundModel Compound { get; set; } // The compound involved in the reaction
        public int StoichiometricCoefficient { get; set; } // The coefficient in front of the compound in the reaction equation
    }
}
