using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.DictionaryFunction
{
    public class Definition
    {
        [Column("Id")]
        public long Id { get; set; }

        [Column("PosId")]
        public long PosId { get; set; }

        [Column("Content")]
        public string Content { get; set; } = string.Empty;
    }
}
