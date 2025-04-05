using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class ElementModel
    {
        public string atomic_number { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public double atomic_mass { get; set; }
        public string color { get; set; }
        public string electron_configuration { get; set; }
        public double? electronegativity { get; set; }
        public int atomic_radius { get; set; }
        public double ionization_energy { get; set; }
        public double? electron_affinity { get; set; }
        public string oxidation_states { get; set; }
        public string phase { get; set; }
        public double melting_point { get; set; }
        public double boiling_point { get; set; }
        public double density { get; set; }
        public string category { get; set; }
        public string discovery_year { get; set; }
        public int row { get; set; }
        public int column { get; set; }
    }
}
