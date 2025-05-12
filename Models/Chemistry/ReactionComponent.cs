using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class ReactionComponent
    {
        public Compound Compound { get; set; } = null!;

        public int StoichiometricCoefficient { get; set; }
    }
}
