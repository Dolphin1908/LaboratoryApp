using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class PhysicalProperty
    {
        public long Id { get; set; }

        public string PropertyName { get; set; }

        public string Unit { get; set; }

        public double Value { get; set; }

        public string Description { get; set; }
    }
}
