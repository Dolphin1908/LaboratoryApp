using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class DefinitionModel
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("pos_id")]
        public long pos_id { get; set; }
        [Column("definition")]
        public string definition { get; set; }
    }
}
