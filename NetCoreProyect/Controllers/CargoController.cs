using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreProyect.Entidades;
using NetCoreProyect.Models;

namespace NetCoreProyect.Controllers
{
    [Route("api2023/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        //Declaramos una variable de solo lectura de tipo 'BD_Empresa2023Context'
        public readonly BD_Empresa2023Context _DBContext;

        //En el constructor de la clase 'EmpresaController' le pasamos como parametro una variable de tipo 'BD_Empresa2023Context'
        public CargoController(BD_Empresa2023Context _context)
        {
            // Para que inicialice el contexto de la base de datos y podamos utilizarlo en varias partes de esta clase 'EmpresaController'
            //ES DECIR, LA CLASE 'EmpresaController' está diseñada para interactuar con la BD a través de la instancia de BD_Empresa2023Context.
            _DBContext = _context;
        }


        [HttpGet("ListaCargo")]
        public ActionResult<IEnumerable<SolicitudCargo>> GetListaCargos()
        {
            var query = from c in _DBContext.Cargos
                        select new SolicitudCargo
                        {
                            Idcargo = c.Idcargo,
                            NomCargo = c.NomCargo,
                            Sueldo = c.Sueldo,
                            Estado = c.Estado
                        };

            return query.ToList();
        }
    }
}
