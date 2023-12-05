namespace NetCoreProyect.Entidades
{
    public class SolicitudEmpleado
    {
        public int IdEmpleado { get; set; }
        public int Idcargo { get; set; }
        public int Iddist { get; set; }
        public string NombreEmp { get; set; }
        public string ApeEmp { get; set; } 
        public string Direccion { get; set; } 
        public string? Telefono { get; set; }
        public string Correo { get; set; }
        public int Estado { get; set; }


    }
}
