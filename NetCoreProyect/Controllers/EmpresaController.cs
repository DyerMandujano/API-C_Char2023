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
                            Idcargo = e.Idcargo,
                            Iddist = e.Iddist,
                            NombreEmp = e.NomEmp,
                            ApeEmp = e.ApeEmp,
                            Direccion = e.Direccion,
                            Telefono = e.Telefono,
                            Correo  = e.Correo,
                            Estado = e.Estado
                        };

            return query.ToList();
        }

        [HttpGet("BuscarPorEmpleado/{id}")]
        public async Task<ActionResult<SolicitudEmpleado>> GetOneEmpleado(int id)
        {
            //creamos la variable 'distritoExistente' para almacenar los valores del objeto dependiendo del id proporcionado en el parametro
            var empleExistente = await _DBContext.Empleados.FirstOrDefaultAsync(d => d.Idemp == id);

            //SI NO ENCUENTRA AL EMPLEADO
            if (empleExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Empleado no encontrado");
            }

            SolicitudEmpleado soli_emple = new()
            {
                IdEmpleado = empleExistente.Idemp,
                Idcargo = empleExistente.Idcargo,
                Iddist = empleExistente.Iddist,
                NombreEmp = empleExistente.NomEmp,
                ApeEmp = empleExistente.ApeEmp,
                Direccion = empleExistente.Direccion,
                Telefono = empleExistente.Telefono,
                Correo = empleExistente.Correo,
                Estado = empleExistente.Estado
            };

            //Retorna el objeto 'soli_emple' con el statusCode200 = OK
            return Ok(soli_emple);
        }

        [HttpPost("CrearEmpleado")]
        public async Task<ActionResult<SolicitudEmpleado>> NuevoEmpleado([FromBody] SolicitudEmpleado solicitudEmpleado)
        {

            if (solicitudEmpleado == null)
            {
                // Devolvemos una statusCode400 = BadRequest
                return BadRequest("La solicitud es nula");
            }

            Empleado newEmpleado = new()
            {
                Idcargo = solicitudEmpleado.Idcargo,
                Iddist = solicitudEmpleado.Iddist,
                NomEmp = solicitudEmpleado.NombreEmp,
                ApeEmp = solicitudEmpleado.ApeEmp,
                Direccion = solicitudEmpleado.Direccion,
                Telefono = solicitudEmpleado.Telefono,
                Correo = solicitudEmpleado.Correo,
                Estado = solicitudEmpleado.Estado
            };

            _DBContext.Empleados.Add(newEmpleado);
            await _DBContext.SaveChangesAsync();

            return Ok("Empleado creado exitosamente");

        }

        [HttpPut("ActualizarEmpleado/{id}")]
        public async Task<ActionResult<SolicitudEmpleado>> UpdateEmpleado(int id, [FromBody] SolicitudEmpleado solicitudEmpleado)
        {

            if (solicitudEmpleado == null)
            {
                // Devolvemos una statusCode400 = BadRequest
                return BadRequest("La solicitud es nula");
            }

            var empleExistente = await _DBContext.Empleados.FirstOrDefaultAsync(d => d.Idemp == id);

            if (empleExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Empleado no encontrado");
            }

            //ACtualizamos los datos 
            empleExistente.Idcargo = solicitudEmpleado.Idcargo;
            empleExistente.Iddist = solicitudEmpleado.Iddist;
            empleExistente.NomEmp = solicitudEmpleado.NombreEmp;
            empleExistente.ApeEmp = solicitudEmpleado.ApeEmp;
            empleExistente.Direccion = solicitudEmpleado.Direccion;
            empleExistente.Telefono = solicitudEmpleado.Telefono;
            empleExistente.Direccion = solicitudEmpleado.Direccion;
            empleExistente.Correo = solicitudEmpleado.Correo;
            empleExistente.Estado = solicitudEmpleado.Estado;

            await _DBContext.SaveChangesAsync();
            return Ok("Empleado Actualizado exitosamente");

        }

        [HttpDelete("EliminarEmpleado/{id}")]
        public async Task<ActionResult<SolicitudEmpleado>> DeleteEmpleado(int id)
        {
            var empleExistente = await _DBContext.Empleados.FirstOrDefaultAsync(d => d.Idemp == id);

            if (empleExistente == null)
            {
                //Devolvemos una statusCode404 = NotFound
                return NotFound("Empleado no encontrado");
            }

            empleExistente.Estado = 0;

            await _DBContext.SaveChangesAsync();
            return Ok("Empleado eliminado exitosamente");

        }

    }
}
