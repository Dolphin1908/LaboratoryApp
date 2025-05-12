using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English.DictionaryFunction
{
    public class Example
    {
        [Column("Id")]
        public long Id { get; set; }

        [Column("DefId")]
        public long DefId { get; set; }

        [Column("Content")]
        public string Content { get; set; } = string.Empty;
    }
}
