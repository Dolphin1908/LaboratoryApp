using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using LaboratoryApp.src.Core.Caches;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class CompoundComponent
    {
        public long ElementId { get; set; }

        public string Quantity { get; set; } = string.Empty;

        [NotMapped]
        public string Formula { get; set; }
    }
}
