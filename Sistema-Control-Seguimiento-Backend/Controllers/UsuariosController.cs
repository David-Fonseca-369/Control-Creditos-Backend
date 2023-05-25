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

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<UsuarioDTO>>> Filtrar([FromQuery] FiltrarDTO filtrarDTO)
        {
            var queryable = context.Usuarios
              .Include(x => x.Rol)
              .AsQueryable();

            if (!string.IsNullOrEmpty(filtrarDTO.Text))
            {
                queryable =
                    queryable.Where(x => x.Nombre.StartsWith(filtrarDTO.Text)
                     || x.ApellidoPaterno.StartsWith(filtrarDTO.Text)
                     || x.ApellidoMaterno.StartsWith(filtrarDTO.Text)
                     || x.Correo.StartsWith(filtrarDTO.Text));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var usuarios = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(usuarios);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioPreviewDTO>> GetById(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario is null)
            {
                return NotFound($"El usuario {id} no existe.");
            }

            return mapper.Map<UsuarioPreviewDTO>(usuario);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {  
            
            //Limpio strings del DTO recibido de usuario
            usuarioCreacionDTO.TrimTextProperties();


            bool validarCorreo = await ValidarCorreo(usuarioCreacionDTO.Correo);
            if (validarCorreo)
            {
                return BadRequest("El correo ya existe.");
            }
         
            CrearPasswordHash(usuarioCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var usuarioMap = mapper.Map<Usuario>(usuarioCreacionDTO);
            usuarioMap.PasswordHash = passwordHash;
            usuarioMap.PasswordSalt = passwordSalt;
            usuarioMap.Estado = true;

            await context.AddAsync(usuarioMap);

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] UsuarioEditarDTO usuarioEditarDTO)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario is null)
            {
                return NotFound($"El usuario {id} no existe.");
            }

            //limpio todas las cajas de la DTO recibida
            usuarioEditarDTO.TrimTextProperties();

            if (usuario.Correo != usuarioEditarDTO.Correo.ToLower())
            {
                bool validarCorreo = await ValidarCorreo(usuarioEditarDTO.Correo.ToLower());

                if (validarCorreo)
                {
                    return BadRequest($"El correo {usuarioEditarDTO.Correo.ToLower()} ya existe.");
                }
            }

            //editamos 
            usuario = mapper.Map(usuarioEditarDTO, usuario);

            //Validar si se ha cambiado el password
            if (!string.IsNullOrEmpty(usuarioEditarDTO.Password))
            {
                //Cambiar contraseña
                CrearPasswordHash(usuarioEditarDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ValidarCorreo(string correo)
        {
            //Validar el de alumno
            bool existeCorreo = await context.Alumnos.AnyAsync(x => x.Correo == correo.ToLower());

            if (existeCorreo)//si existe ya retorno que hay uno existente.
            {
                return existeCorreo;
            }

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
