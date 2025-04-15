using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class WordModel
    {
        [Column("Id")]
        public long Id { get; set; }
        [Column("Word")]
        public string Word { get; set; }
        [Column("Prononciation")]
        public string Prononciation { get; set; }
    }
}
