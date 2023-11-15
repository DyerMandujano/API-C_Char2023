using System;
using System.Collections.Generic;

namespace NetCoreProyect.Models
{
    public partial class Distrito
    {
        public Distrito()
        {
            Empleados = new HashSet<Empleado>();
        }

        public int Iddist { get; set; }
        public string NomDist { get; set; } = null!;
        public int Estado { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
