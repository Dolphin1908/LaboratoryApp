using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class PosModel
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("word_id")]
        public long word_id { get; set; }
        [Column("pos")]
        public string pos { get; set; }
    }
}
