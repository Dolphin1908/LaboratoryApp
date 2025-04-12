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
        [Column("Id")]
        public long Id { get; set; }
        [Column("PosId")]
        public long PosId { get; set; }
        [Column("Definition")]
        public string Definition { get; set; }
    }
}
