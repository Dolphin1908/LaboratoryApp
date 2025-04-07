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
        [Column("id")]
        public long Id { get; set; }
        [Column("headword")]
        public string headword { get; set; }
        [Column("phonetic")]
        public string phonetic { get; set; }
    }
}
