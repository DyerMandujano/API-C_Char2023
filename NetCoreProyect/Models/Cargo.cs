using System;
using System.Collections.Generic;

namespace NetCoreProyect.Models
{
    public partial class Cargo
    {
        public Cargo()
        {
            Empleados = new HashSet<Empleado>();
        }

        public int Idcargo { get; set; }
        public string NomCargo { get; set; } = null!;
        public decimal Sueldo { get; set; }
        public int Estado { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
