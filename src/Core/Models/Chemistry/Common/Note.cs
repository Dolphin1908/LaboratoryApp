using LaboratoryApp.src.Core.Models.Chemistry.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public abstract class Notes
    {
        public List<string> Content { get; set; } = new List<string>();
    }

    public class CompoundNote : Notes
    {
        public CompoundNoteType NoteType { get; set; }
    }

    public class ReactionNote : Notes
    {
        public ReactionNoteType NoteType { get; set; }
    }
}
