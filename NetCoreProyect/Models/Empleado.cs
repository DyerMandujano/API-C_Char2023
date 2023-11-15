using System;
using System.Collections.Generic;

namespace NetCoreProyect.Models
{
    public partial class Empleado
    {
        public int Idemp { get; set; }
        public int Idcargo { get; set; }
        public int Iddist { get; set; }
        public string NomEmp { get; set; } = null!;
        public string ApeEmp { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Correo { get; set; } = null!;
        public int Estado { get; set; }

        public virtual Cargo IdcargoNavigation { get; set; } = null!;
        public virtual Distrito IddistNavigation { get; set; } = null!;
    }
}
