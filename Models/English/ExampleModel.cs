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
        [Column("Id")]
        public long Id { get; set; }
        [Column("DefId")]
        public long DefId { get; set; }
        [Column("Example")]
        public string Example { get; set; }
    }
}
