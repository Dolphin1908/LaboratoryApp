using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English.DTOs
{
    public class WordResultDTO
    {
        public long Id { get; set; }
        public string Word { get; set; }
        public string Pronunciation { get; set; }
        public List<PosDTO> Pos { get; set; }
    }
    public class PosDTO
    {
        public long Id { get; set; }
        public long WordId { get; set; }
        public string Pos { get; set; }
        public List<DefinitionDTO> Definitions { get; set; }
    }

    public class DefinitionDTO
    {
        public long Id { get; set; }
        public long PosId { get; set; }
        public string Definition { get; set; }
        public List<ExampleDTO> Examples { get; set; }
    }

    public class ExampleDTO
    {
        public long Id { get; set; }
        public long DefId { get; set; }
        public string Example { get; set; }
        public string Translation { get; set; }
    }
}
