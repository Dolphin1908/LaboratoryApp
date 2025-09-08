using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry
{
    public class ChemicalProperty
    {
        public string PropertyName { get; set; }

        public string Description { get; set; }
    }
}
