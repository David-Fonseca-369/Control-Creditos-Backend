using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Seguimiento_Backend.DTOs;
using Sistema_Control_Seguimiento_Backend.DTOs.Cursos;
using Sistema_Control_Seguimiento_Backend.DTOs.Solicitudes;
using Sistema_Control_Seguimiento_Backend.Entities;
using Sistema_Control_Seguimiento_Backend.Helpers;

namespace Sistema_Control_Seguimiento_Backend.Controllers
{
    [Authorize]
    [Route("api/solicitudes")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public SolicitudesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] SolicitudCreacionDTO solicitudCreacionDTO)
        {
            //var si no existe ya el registro
            var solicitud = await context.Solicitudes
                .FirstOrDefaultAsync(x => x.IdAlumno == solicitudCreacionDTO.IdAlumno
                && x.IdCurso == solicitudCreacionDTO.IdCurso);


            if (solicitud is not null)
            {
                return BadRequest("Sólo puedes generar una solicitud.");
            }

            solicitudCreacionDTO.TrimTextProperties();

            var nuevaSolicitud = new Solicitud
            {
                IdCurso = solicitudCreacionDTO.IdCurso,
                IdAlumno = solicitudCreacionDTO.IdAlumno,
                Estado = 0,//solicitud pendiente                
                FechaCreacion = DateTime.Now,
                ComentariosSolicitud = solicitudCreacionDTO.ComentariosSolicitud
            };

            await context.AddAsync(nuevaSolicitud);
            await context.SaveChangesAsync();

            return NoContent();
        }


        //GET api/curso/SolicitudeAlumnoPaginacion
        [HttpGet("solicitudesAlumnoPaginacion/{idAlumno:int}")]
        public async Task<ActionResult<List<SolicitudDTO>>> SolicitudesAlumnoPaginacion(int idAlumno, [FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Solicitudes
                .Include(x => x.Curso)
                .Include(x => x.Curso.CreadoPor)
                .Where(x => x.IdAlumno == idAlumno)
                .OrderByDescending(x => x.FechaCreacion)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var solicitudes = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<SolicitudDTO>>(solicitudes);
        }

        //GET api/curso/SolicitudeAlumnoFiltrar
        [HttpGet("solicitudesAlumnoFiltrar/{idAlumno:int}")]
        public async Task<ActionResult<List<SolicitudDTO>>> SolicitudesAlumnoFiltrar(int idAlumno, [FromQuery] FiltrarDTO filtrarDTO)
        {
            var queryable = context.Solicitudes
                .Include(x => x.Curso)
                .Include(x => x.Curso.CreadoPor)
                .Where(x => x.IdAlumno == idAlumno)
                .OrderByDescending(x => x.FechaCreacion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtrarDTO.Text))
            {
                queryable = queryable.Where(x => x.Curso.Nombre.StartsWith(filtrarDTO.Text)
                || x.Curso.CreadoPor.Nombre.StartsWith(filtrarDTO.Text)
                || x.Curso.CreadoPor.ApellidoPaterno.StartsWith(filtrarDTO.Text)
                || x.Curso.CreadoPor.ApellidoMaterno.StartsWith(filtrarDTO.Text)
                );
            }

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var solicitudes = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<SolicitudDTO>>(solicitudes);
        }


        //GET api/curso/SolicitudeInstructorPagiancion/{id}
        [HttpGet("solicitudesInstructorPaginacion/{idInstructor:int}")]
        public async Task<ActionResult<List<SolicitudInstructorDTO>>> SolicitudesInstrcutorPaginacion(int idInstructor, [FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Solicitudes
                .Include(x => x.Curso)
                .Include(x => x.Alumno)
                .Include(x => x.Curso.CreadoPor)
                //solo los que esten pendientes
                .Where(x => x.Curso.CreadoPor.Id == idInstructor && x.Estado == 0)
                .OrderByDescending(x => x.FechaCreacion)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var solicitudes = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<SolicitudInstructorDTO>>(solicitudes);
        }

    }
}
