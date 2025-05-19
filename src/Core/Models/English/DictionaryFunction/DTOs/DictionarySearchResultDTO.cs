using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.DictionaryFunction.DTOs
{
    public class DictionarySearchResultDTO
    {
        public long WordId { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Pos { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
    }
}
