using LaboratoryApp.src.Core.Models.Chemistry.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class Reaction
    {
        public long Id { get; set; }

        public List<ReactionComponent> Reactants { get; set; } = new List<ReactionComponent>();

        public List<ReactionComponent> Products { get; set; } = new List<ReactionComponent>();

        public List<ReactionType> ReactionType { get; set; } = new List<ReactionType>();

        public double? YieldPercent { get; set; } // 0-100

        public DateTime? ReactionDate { get; set; } // DateTime

        public ReactionCondition Condition { get; set; } = new ReactionCondition();

        public List<ReactionNote> ReactionNotes { get; set; } = new List<ReactionNote>();
    }
}
