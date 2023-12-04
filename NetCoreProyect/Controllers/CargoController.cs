using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("BuscarPorCargo/{id}")]
        public async Task<ActionResult<SolicitudCargo>> GetOneCargo(int id)
        {
            //creamos la variable 'cargoExistente' para almacenar los valores del objeto dependiendo del id proporcionado en el parametro
            var cargoExistente = await _DBContext.Cargos.FirstOrDefaultAsync(d => d.Idcargo == id);

            //SI NO ENCUENTRA EL Cargo
            if (cargoExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Cargo no encontrado");
            }


            //Aca se utiliza la inicialización de objeto en línea
            SolicitudCargo soli_cargo = new()
            {
               //ENTIDAD(soli_cargo)    -      MODELO
                Idcargo = cargoExistente.Idcargo,
                NomCargo = cargoExistente.NomCargo,
                Sueldo = cargoExistente.Sueldo,
                Estado = cargoExistente.Estado
            };

            return Ok(soli_cargo);

        }

        [HttpPost("CrearCargo")]
        public async Task<ActionResult> NuevoCargo([FromBody] SolicitudCargo solicitudCargo)
        {
            if (solicitudCargo == null)
            {
                // Devolvemos una statusCode400 = BadRequest
                return BadRequest("La solicitud es nula");
            }
            else if (solicitudCargo.NomCargo == "" || solicitudCargo.Sueldo == 0)
            {
                // Devolvemos una statusCode400 = BadRequest
                return BadRequest("Ingrese correctamente los datos");
            }


            var nuevoCargo = new Cargo
            {
                NomCargo = solicitudCargo.NomCargo,
                Sueldo= solicitudCargo.Sueldo,
                Estado = solicitudCargo.Estado
            };

            //NOTA: Las operaciones que modifican el CONTEXTO de la base de datos, como agregar, actualizar o eliminar entidades,
            //son operaciones síncronas y no requieren await.
            //Agregamos un nuevo objeto al 'CONTEXTO de la BD'. 
            _DBContext.Cargos.Add(nuevoCargo);

            //NOTA: Por otro lado, Las operaciones que realizan cambios en la BD, como 'SaveChangesAsync()'son operaciones asincrónicas y deben usar await para esperar a que se completen.  
            //Esta operación guarda los cambios que se han hecho en la BD, es por ello que aqui utilizamos el 'await'
            //para esperar a que esta operación se complete antes de continuar con el resto del codigo.
            await _DBContext.SaveChangesAsync();

            //Devolvemos una statusCode200 = OK
            return Ok("Cargo creado exitosamente");
        }

        [HttpPut("ActualizarCargo/{id}")]
        public async Task<ActionResult> UpdateCargo(int id, [FromBody] SolicitudCargo solicitudCargo)
        {
            if (solicitudCargo == null)
            {
                //Devolvemos una statusCode400 = BadRequest
                return BadRequest("La solicitud es nula");
            }

            var cargoExistente = await _DBContext.Cargos.FirstOrDefaultAsync(d => d.Idcargo == id);

            if (cargoExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Cargo no encontrado");
            }

            //Actualiza las propiedades del objeto 'cargoExistente' con los valores del obj 'solicitudCargo'
            cargoExistente.NomCargo = solicitudCargo.NomCargo;
            cargoExistente.Sueldo = solicitudCargo.Sueldo;
            cargoExistente.Estado = solicitudCargo.Estado;

            //Guarda los cambios en la BD
            await _DBContext.SaveChangesAsync();

            //Devolvemos una statusCode200 = Ok
            return Ok("Cargo actualizado exitosamente");
        }

        [HttpDelete("EliminarCargo/{id}")]
        public async Task<ActionResult> DeleteCargo(int id)
        {
            var cargoExistente = _DBContext.Cargos.FirstOrDefault(d => d.Idcargo == id);

            if (cargoExistente == null)
            {
                return NotFound("Cargo no encontrado");
            }

            // Elimina el Cargo de la BD
            cargoExistente.Estado = 0;
            await _DBContext.SaveChangesAsync();

            return Ok("Cargo eliminado exitosamente");
        }
    }
}
