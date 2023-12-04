using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreProyect.Entidades;
using NetCoreProyect.Models;
using System.Collections.Generic;

namespace NetCoreProyect.Controllers
{
    //ES LA RUTA PRINCIPAL
    [Route("api2023/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        //Declaramos una variable de solo lectura de tipo 'BD_Empresa2023Context'
        public readonly BD_Empresa2023Context _DBContext;

        //En el constructor de la clase 'EmpresaController' le pasamos como parametro una variable de tipo 'BD_Empresa2023Context'
        public EmpresaController(BD_Empresa2023Context _context)
        {
            // Para que inicialice el contexto de la base de datos y podamos utilizarlo en varias partes de esta clase 'EmpresaController'
            //ES DECIR, LA CLASE 'EmpresaController' está diseñada para interactuar con la BD a través de la instancia de BD_Empresa2023Context.
            _DBContext = _context;
        }


        [HttpGet("ListaEmpleados")]
        public ActionResult<IEnumerable<SolicitudEmpleado>> GetListaEmpleados()
        {
            var query = from e in _DBContext.Empleados
                        select new SolicitudEmpleado
                        {
                            IdEmpleado = e.Idemp,
                            NombreEmp = e.NomEmp,
                        };

            return query.ToList();
        }

        

       

    }
}
