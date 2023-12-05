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

        [HttpGet("BuscarPorDistrito/{id}")]
        public async Task<ActionResult<SolicitudDistrito>> GetOneDistrito(int id)
        {
            //creamos la variable 'distritoExistente' para almacenar los valores del objeto dependiendo del id proporcionado en el parametro
            var distritoExistente = await _DBContext.Distritos.FirstOrDefaultAsync(d => d.Iddist == id);

            //SI NO ENCUENTRA EL DISTRITO
            if (distritoExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Distrito no encontrado");
            }

            //NOTA: PODEMOS INSTANCIAR DE DOS MANERAS LA CLASE 'SolicitudDistrito'
            //1era Forma - Tradicional
            //SolicitudDistrito soli_dist = new SolicitudDistrito();

            //UNA VEZ QUE SE HAYA ENCONTRADO EL DISTRITO DESEADO POR EL PARAMETRO id, cada valor que tiene cada propiedad del objeto 'distritoExistente'(TRABAJA CON EL CONTEXTO DE LA DB DE SU MODELO 'Distrito')
            //Se almacenará en las propiedades del objeto 'soli_dist'(TRABAJA CON LA ENTIDAD 'SolicitudDistrito')
            //    ENTIDAD    -      MODELO
            //
            //soli_dist.Iddist = distritoExistente.Iddist;
            //soli_dist.NomDist = distritoExistente.NomDist;
            //soli_dist.Estado = distritoExistente.Estado;

            //2da Forma - object initializer
            //Aca se utiliza la inicialización de objeto en línea
            SolicitudDistrito soli_dist = new()
            {
              //ENTIDAD    -      MODELO
                Iddist = distritoExistente.Iddist,
                NomDist = distritoExistente.NomDist,
                Estado = distritoExistente.Estado,
            };

            //Retorna el objeto 'soli_dist' con el statusCode200 = OK
            return Ok(soli_dist);

        }

        [HttpPost("CrearDistrito")]
        //El body que le voy a enviar como parametro a este metodo va a ser un objeto de tipo 'SolicitudDistrito'
        public async Task<ActionResult> NuevoDistrito([FromBody] SolicitudDistrito solicitudDistrito)
        {
            if (solicitudDistrito == null)
            {
                // Devolvemos una statusCode400 = BadRequest
                return BadRequest("La solicitud es nula");
            }
            else if(solicitudDistrito.NomDist == "" || solicitudDistrito.NomDist == null)
            {
                // Devolvemos una statusCode400 = BadRequest
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

            //Devolvemos una statusCode200 = OK
            return Ok("Distrito creado exitosamente");
        }

        [HttpPut("ActualizarDistrito/{id}")]
        //El body que le voy a enviar como parametro a este metodo va a ser un objeto de tipo 'SolicitudDistrito' y ademas un id
        public async Task<ActionResult> UpdateDistrito(int id, [FromBody] SolicitudDistrito solicitudDistrito)
        {
            if (solicitudDistrito == null)
            {
                //Devolvemos una statusCode400 = BadRequest
                return BadRequest("La solicitud es nula");
            }

            //creamos la variable 'distritoExistente' para almacenar los valores del objeto dependiendo del id proporcionado en el parametro
            var distritoExistente = await _DBContext.Distritos.FirstOrDefaultAsync(d => d.Iddist == id);

            if (distritoExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Distrito no encontrado");
            }

            //Actualiza las propiedades del objeto 'distritoExistente' con los valores del obj 'solicitudDistrito'
            distritoExistente.NomDist = solicitudDistrito.NomDist;
            distritoExistente.Estado = solicitudDistrito.Estado;

            //Guarda los cambios en la BD
            await _DBContext.SaveChangesAsync();

            //Devolvemos una statusCode200 = Ok
            return Ok("Distrito actualizado exitosamente");
        }

        [HttpDelete("EliminarDistrito/{id}")]
        public async Task<ActionResult> DeleteDistrito(int id)
        {
            //creamos la variable 'distritoExistente' para almacenar los valores del objeto dependiendo del id proporcionado en el parametro
            var distritoExistente = _DBContext.Distritos.FirstOrDefault(d => d.Iddist == id);

            if (distritoExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Distrito no encontrado");
            }

            // Elimina el Distrito de la BD
            distritoExistente.Estado = 0;
            //Guardamos los cambias en la BD
            await _DBContext.SaveChangesAsync();

            //Devolvemos una statusCode200 = Ok
            return Ok("Distrito eliminado exitosamente");
        }

    }
}
