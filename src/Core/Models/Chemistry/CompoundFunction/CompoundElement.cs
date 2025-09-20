using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class CompoundElement
    {
        public long ElementId { get; set; } // The element in the compound

        public string Quantity { get; set; } = string.Empty;
    }
}
