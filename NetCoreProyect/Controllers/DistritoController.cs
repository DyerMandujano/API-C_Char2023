using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreProyect.Entidades;
using NetCoreProyect.Models;

namespace NetCoreProyect.Controllers
{
    [Route("api2023/[controller]")]
    [ApiController]
    public class DistritoController : ControllerBase
    {
        //Declaramos una variable de solo lectura de tipo 'BD_Empresa2023Context'
        public readonly BD_Empresa2023Context _DBContext;

        //En el constructor de la clase 'EmpresaController' le pasamos como parametro una variable de tipo 'BD_Empresa2023Context'
        public DistritoController(BD_Empresa2023Context _context)
        {
            // Para que inicialice el contexto de la base de datos y podamos utilizarlo en varias partes de esta clase 'EmpresaController'
            //ES DECIR, LA CLASE 'EmpresaController' está diseñada para interactuar con la BD a través de la instancia de BD_Empresa2023Context.
            _DBContext = _context;
        }

        //LOS METODOS HTTP SE REALIZAN DE FORMA ASINCRONA YA QUE SE TIENEN QUE PROCESAR MULTIPLES SOLICITUDES SIMULTANEAMENTE. POR LO QUE
        //EL USO DE FORMA ASINCRONA PERMITE UN MEJOR MANEJO DE LAS SOLICITUDES CONCURRENTES SIN BLOQUEAR HILOS.
        [HttpGet("ListaDistrito")]

        //El ActionResult devuelve valores al controlador en respuesta a una solicitud HTTP.
       
        //El 'IEnumerable' devuelve una colección de objetos de tipo 'SolicitudDistrito'. Ademas esta interfaz está
        //implementada en la clase List por lo que podemos utilizarla. Es por ello que en la linea 37, el 'IEnumerable' la convertimos en 'ToList()'
        public async Task<ActionResult<IEnumerable<SolicitudDistrito>>> GetListaDistritos()
        {
            var query =  from d in _DBContext.Distritos
                        select new SolicitudDistrito
                        {
                            Iddist = d.Iddist,
                            NomDist = d.NomDist,
                            Estado = d.Estado,
                        };

           //En esta caso se esta retornando un objeto 'ActionResult' que contiene una colección de objetos de tipo 'SolicitudDistrito'.
            return await query.ToListAsync();
        }

        [HttpPost("CrearDistrito")]
        public async Task<ActionResult> NuevoDistrito([FromBody] SolicitudDistrito solicitudDistrito)
        {
            if (solicitudDistrito == null)
            {
                return BadRequest("La solicitud es nula");
            }
            else if(solicitudDistrito.NomDist == "" || solicitudDistrito.NomDist == null)
            {
                return BadRequest("Ingrese un Distrito");
            }


            var nuevoDistrito = new Distrito
            {
                NomDist = solicitudDistrito.NomDist,
                Estado = solicitudDistrito.Estado,
            };

            //NOTA: Las operaciones que modifican el CONTEXTO de la base de datos, como agregar, actualizar o eliminar entidades,
            //son operaciones síncronas y no requieren await.
            //Agregamos un nuevo objeto al 'CONTEXTO de la BD'. 
            _DBContext.Distritos.Add(nuevoDistrito);
            
            //NOTA: Por otro lado, Las operaciones que realizan cambios en la BD, como 'SaveChangesAsync()'son operaciones asincrónicas y deben usar await para esperar a que se completen.  
            //Esta operación guarda los cambios que se han hecho en la BD, es por ello que aqui utilizamos el 'await'
            //para esperar a que esta operación se complete antes de continuar con el resto del codigo.
            await _DBContext.SaveChangesAsync();

            // Devolvemos una statusCode200 = OK
            return Ok("Distrito creado exitosamente");
        }

        
    }
}
