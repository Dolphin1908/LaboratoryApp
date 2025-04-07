using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.DTOs
{
    public class SearchResultDTO
    {
        public long WordId { get; set; }
        public string Word { get; set; }
        public string Pos { get; set; }
        public string Meaning { get; set; }
    }
}
