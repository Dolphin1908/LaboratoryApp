using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class ExampleModel
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("def_id")]
        public long def_id { get; set; }
        [Column("example")]
        public string example { get; set; }
    }
}
