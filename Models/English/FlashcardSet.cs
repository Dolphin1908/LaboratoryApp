using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class FlashcardSet
    {
        public long id { get; set; }
        public string name { get; set; }
        public List<FlashcardModel> flashcards { get; set; }
    }
}
