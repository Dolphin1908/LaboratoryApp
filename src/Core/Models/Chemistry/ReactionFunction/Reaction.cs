using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaboratoryApp.src.Core.Helpers;
using LaboratoryApp.src.Core.Models.Chemistry.Enums;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class Reaction
    {
        public long Id { get; set; }

        public long OwnerId { get; set; } // User who created or owns the reaction

        public List<ReactionComponent> Reactants { get; set; } = new List<ReactionComponent>();

        public List<ReactionComponent> Products { get; set; } = new List<ReactionComponent>();

        public List<ReactionType> ReactionType { get; set; } = new List<ReactionType>();

        public string YieldPercent { get; set; } = string.Empty; // 0-100

        public DateTime? ReactionDate { get; set; } // DateTime

        public ReactionCondition Condition { get; set; } = new ReactionCondition();

        public List<ReactionNote> ReactionNotes { get; set; } = new List<ReactionNote>();

        // Support
        [NotMapped]
        public string ReactionTypeDescription =>
            ReactionType == null || ReactionType.Count == 0
                ? string.Empty
                : string.Join(", ", ReactionType.Select(t => t.GetDisplayName()));

        [NotMapped]
        public string ReactantDescription =>
            Reactants == null || Reactants.Count == 0
                ? string.Empty
                : string.Join(", ", Reactants.Select(r => r.Formula));

        [NotMapped]
        public string ProductDescription =>
            Products == null || Products.Count == 0
                ? string.Empty
                : string.Join(", ", Products.Select(p => p.Formula));

        [NotMapped]
        public string DisplayEquation => $"{string.Join(" + ", Reactants.Select(r => r.Display))} → {string.Join(" + ", Products.Select(p => p.Display))}";

        [NotMapped]
        public List<string> ReactantFormulas => Reactants.Select(r => r.Formula)
                                                         .Where(f => !string.IsNullOrEmpty(f))
                                                         .ToList();

        [NotMapped]
        public List<string> ProductFormulas => Products.Select(p => p.Formula)
                                                       .Where(f => !string.IsNullOrEmpty(f))
                                                       .ToList();

    }
}
