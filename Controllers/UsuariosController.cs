using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubenEjercicio.Data;
using RubenEjercicio.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace RubenEjercicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        // este es el constructor que sirve para que trabaje con la base de datos 
        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }


        // este es el metodo que te muestra todos los usuarios    
        [HttpGet]

        public IActionResult GetUsuarios()
        {

            var usuarios = _context.Usuarios.ToList();

            return Ok(usuarios);

        }

        [HttpPost]

        public IActionResult CrearUsuarios([FromBody] Usuarios usuarios)
        {

            if (string.IsNullOrEmpty(usuarios.Nombre))
            { 
                return BadRequest("El nombre es obligatorio");
            }

            if (string.IsNullOrEmpty(usuarios.Email))
            {
               return BadRequest("el email es obligatorio");
            }
            
            if (usuarios.Edad <= 0)
            {
                return BadRequest("la edad debe ser mayor a 0 ");
            }

            if (string.IsNullOrWhiteSpace(usuarios.Email) ||
                        !Regex.IsMatch(usuarios.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Email inválido");
            }

            usuarios.FechaCreacion = DateTime.Now;

            _context.Usuarios.Add(usuarios);
            _context.SaveChanges();

            return Created("", usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerUsuarioPorId(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {

                return NotFound("Usuario no encontrado"); // 404
            }

            return Ok(usuario); // 200

        }

        [HttpPut("{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] Usuarios usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Email))
            {
                return BadRequest("Datos inválidos"); // 400
            }

            var usuarioExistente = _context.Usuarios.Find(id);

            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado"); // 404
            }

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Edad = usuario.Edad;

            _context.SaveChanges(); // guarda en la DB

            return Ok(usuarioExistente); // 200
    
        }

        [HttpDelete("{id}")]

        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado"); // 404
            }

            _context.Usuarios.Remove(usuario); // elimina de la DB
            _context.SaveChanges();            // guarda cambios

            return NoContent(); // 204 (mejor práctica)
        }
    }
         






}
        