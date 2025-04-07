using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.DTOs
{
    class WordResultDTO
    {
        public long id { get; set; }
        public string word { get; set; }
        public string pronunciation { get; set; }
        public List<PosDTO> pos { get; set; }
    }
    public class PosDTO
    {
        public long id { get; set; }
        public long word_id { get; set; }
        public string pos { get; set; }
        public List<DefinitionDTO> definitions { get; set; }
    }

    public class DefinitionDTO
    {
        public long id { get; set; }
        public long pos_id { get; set; }
        public string definition { get; set; }
        public List<ExampleDTO> examples { get; set; }
    }

    public class ExampleDTO
    {
        public long id { get; set; }
        public long def_id { get; set; }
        public string example { get; set; }
    }
}
