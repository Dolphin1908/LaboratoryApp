using LaboratoryApp.Models.Chemistry.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public abstract class Note
    {
        public string Content { get; set; } = String.Empty;
    }

    public class CompoundNote : Note
    {
        public CompoundNoteType NoteType { get; set; }
    }

    public class ReactionNote : Note
    {
        public ReactionNoteType NoteType { get; set; }
    }
}
