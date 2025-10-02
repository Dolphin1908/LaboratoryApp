using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Helper
{
    public class CounterModel
    {
        public long Id { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public long Seq { get; set; }
    }
}
