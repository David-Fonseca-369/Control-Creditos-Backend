using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Seguimiento_Backend.DTOs;
using Sistema_Control_Seguimiento_Backend.DTOs.Alumnos;
using Sistema_Control_Seguimiento_Backend.DTOs.Usuarios;
using Sistema_Control_Seguimiento_Backend.Entities;
using Sistema_Control_Seguimiento_Backend.Helpers;

namespace Sistema_Control_Seguimiento_Backend.Controllers
{

    [Route("api/alumnos")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AlumnosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<AlumnoDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Alumnos.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var alumnos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<AlumnoDTO>>(alumnos);
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<AlumnoDTO>>> Filtrar([FromQuery] FiltrarDTO filtrarDTO)
        {
            var queryable = context.Alumnos.AsQueryable();

            if (!string.IsNullOrEmpty(filtrarDTO.Text))
            {
                queryable =
                    queryable.Where(x => x.Nombre.StartsWith(filtrarDTO.Text)
                     || x.ApellidoPaterno.StartsWith(filtrarDTO.Text)
                     || x.ApellidoMaterno.StartsWith(filtrarDTO.Text)
                     || x.Correo.StartsWith(filtrarDTO.Text)
                     || x.NoCuenta.StartsWith(filtrarDTO.Text));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var alumnos = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<AlumnoDTO>>(alumnos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AlumnoPreviewDTO>> GetById(int id)
        {
            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.Id == id);
            
            if (alumno is null)
            {
                return NotFound($"El alumno {id} no existe.");
            }

            return mapper.Map<AlumnoPreviewDTO>(alumno);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Creat([FromBody] AlumnoCreacionDTO alumnoCreacionDTO)
        {
            //limpio campos de tipo string
            alumnoCreacionDTO.TrimTextProperties();

            bool validarCorreo = await ValidarCorreo(alumnoCreacionDTO.Correo);
            
            if (validarCorreo)
            {
                return BadRequest("El correo ya existe.");
            }

            bool validarNocuenta = await ValidarNoCuenta(alumnoCreacionDTO.NoCuenta.ToUpper());
            if (validarNocuenta)
            {
                return BadRequest($"El número de cuenta {alumnoCreacionDTO.NoCuenta.ToUpper()} ya existe.");
            }

            CrearPasswordHash(alumnoCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var alumnoMap = mapper.Map<Alumno>(alumnoCreacionDTO);
            alumnoMap.NoCuenta = alumnoCreacionDTO.NoCuenta.ToUpper();
            alumnoMap.PasswordHash = passwordHash;
            alumnoMap.PasswordSalt = passwordSalt;
            alumnoMap.Estado = true;

            await context.AddAsync(alumnoMap);
            await context.SaveChangesAsync();

            return NoContent(); 
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar (int id, [FromBody] AlumnoEditarDTO alumnoEditarDTO)
        {
            //limpio cajas 
            alumnoEditarDTO.TrimTextProperties();

            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.Id == id);
            if (alumno is null)
            {
                return NotFound($"El alumno {id} no existe.");
            }

            //valido correo
            if (alumno.Correo != alumnoEditarDTO.Correo.ToLower())
            {
                bool validarCorreo = await ValidarCorreo(alumnoEditarDTO.Correo.ToLower());

                if (validarCorreo)
                {
                    return BadRequest($"El correo {alumnoEditarDTO.Correo.ToLower()} ya existe.");
                }
            }

            //valido No. cuenta
            if (alumno.NoCuenta != alumnoEditarDTO.NoCuenta.ToUpper())
            {
                bool validarNocuenta = await ValidarNoCuenta(alumno.NoCuenta.ToUpper());
                if (validarNocuenta)
                {
                    return BadRequest($"El número de cuenta {alumnoEditarDTO.NoCuenta.ToUpper()} ya existe.");
                }
            }

            //editar
            alumno = mapper.Map(alumnoEditarDTO, alumno);

            //se cambio password?
            //Validar si se ha cambiado el password
            if (!string.IsNullOrEmpty(alumnoEditarDTO.Password))
            {
                //Cambiar contraseña
                CrearPasswordHash(alumnoEditarDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                alumno.PasswordHash = passwordHash;
                alumno.PasswordSalt = passwordSalt;
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
        
        
        private async Task<bool> ValidarNoCuenta(string noCuenta)
        {
            return await context.Alumnos.AnyAsync(x => x.NoCuenta == noCuenta);
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
