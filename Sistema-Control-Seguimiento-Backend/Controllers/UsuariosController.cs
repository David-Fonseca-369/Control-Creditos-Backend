using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Seguimiento_Backend.DTOs;
using Sistema_Control_Seguimiento_Backend.DTOs.Usuarios;
using Sistema_Control_Seguimiento_Backend.Entities;
using Sistema_Control_Seguimiento_Backend.Helpers;

namespace Sistema_Control_Seguimiento_Backend.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UsuariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<UsuarioDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Usuarios
                .Include(x => x.Rol)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var usuarios = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(usuarios);

        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            bool validarCorreo = await ValidarCorreo(usuarioCreacionDTO.Correo);
            if (validarCorreo)
            {
                return BadRequest("El correo ya existe.");
            }

            //Limpio strings del DTO recibido de usuario
            usuarioCreacionDTO.TrimTextProperties();

            CrearPasswordHash(usuarioCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var usuarioMap = mapper.Map<Usuario>(usuarioCreacionDTO);            
            usuarioMap.PasswordHash = passwordHash;
            usuarioMap.PasswordSalt = passwordSalt;
            usuarioMap.Estado = true;
            

            await context.AddAsync(usuarioMap);

            await context.SaveChangesAsync();

            return NoContent();

        }

        private async Task<bool> ValidarCorreo(string correo)
        {
            //Validar el de alumno
            //bool existeCorreo = await context.Usuarios.AnyAsync(x => x.Correo == correo.ToLower());

            //if (existeCorreo)//si existe ya retorno que hay uno existente.
            //{
            //    return existeCorreo;
            //}

            //Paso a verificar el de usuario en caso de que no exista en el de alumno
            return await context.Usuarios.AnyAsync(x => x.Correo == correo.ToLower());
        }

        private static void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; //Aquí envía la llave
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //Envíar el password encriptado.
            }
        }


    }
}
