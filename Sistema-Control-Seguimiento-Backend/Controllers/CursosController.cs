using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Seguimiento_Backend.DTOs;
using Sistema_Control_Seguimiento_Backend.DTOs.ArchivosCursos;
using Sistema_Control_Seguimiento_Backend.DTOs.Cursos;
using Sistema_Control_Seguimiento_Backend.Entities;
using Sistema_Control_Seguimiento_Backend.Helpers;
using Sistema_Control_Seguimiento_Backend.Token;

namespace Sistema_Control_Seguimiento_Backend.Controllers
{
    [Authorize]
    [Route("api/cursos")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserSession userSession;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "fondos_cursos";
        private readonly string contenedorArchivos = "archivos_curso";

        public CursosController(ApplicationDbContext context, IMapper mapper, IUserSession userSession, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.userSession = userSession;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        //GET api/curso/TodosPaginacion
        [HttpGet("todosPaginacion/{idUsuario:int}")]
        public async Task<ActionResult<List<CursoDTO>>> TodosPaginacion(int idUsuario, [FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Cursos
                .Where(x => x.IdCreadoPor == idUsuario)
                .OrderByDescending(x => x.UltimaModificacion)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var cursos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<CursoDTO>>(cursos);
        }
        
        //GET api/curso/PublicosPaginacion
        [HttpGet("publicosPaginacion/{idAlumno:int}")]
        public async Task<ActionResult<List<CursoPublicoDTO>>> PublicosPaginacion(int idAlumno, [FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Cursos
                .Include(x => x.CreadoPor)
                .Where(x => x.Estado == 1) //publico
                .OrderByDescending(x => x.UltimaModificacion)
                .AsQueryable();

            //consultar si ya se ha creado una soolicitud (solicitudes) por medio de un 

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var cursos = await queryable.Paginar(paginacionDTO).ToListAsync();

            var cursosPublicosDTO = new List<CursoPublicoDTO>();

            foreach (var curso in cursos)
            {
                var cursoPublicoDTO = mapper.Map<CursoPublicoDTO>(curso);
                var solicitudEnviada = await context.Solicitudes
                    //vemos si ya está registrado aquí el alumno
                    .AnyAsync(s => s.IdAlumno == idAlumno && s.IdCurso == curso.Id);
                    cursoPublicoDTO.SolicitudEnviada = solicitudEnviada;
                    cursosPublicosDTO.Add(cursoPublicoDTO);
            }

            return cursosPublicosDTO;
        }



        [HttpGet("filtrar/{idUsuario:int}")]
        public async Task<ActionResult<List<CursoDTO>>> Filtrar(int idUsuario, [FromQuery] CursosFiltrarDTO cursosFiltrarDTO)
        {
            var queryable = context.Cursos
               .Where(x => x.IdCreadoPor == idUsuario)
               .OrderByDescending(x => x.UltimaModificacion)
               .AsQueryable();

            if (!string.IsNullOrEmpty(cursosFiltrarDTO.Text))
            {
                queryable = queryable.Where(x => x.Nombre.StartsWith(cursosFiltrarDTO.Text));
            }

            if (cursosFiltrarDTO.Estado == 0) //Pendiente
            {
                queryable = queryable.Where(x => x.Estado == 0);
            }
            
            if (cursosFiltrarDTO.Estado == 1) //Publico
            {
                queryable = queryable.Where(x => x.Estado == 1);
            }

            await HttpContext.InsertarParametrosPaginacionEnCabeceraAsync(queryable);

            var cursos = await queryable.Paginar(cursosFiltrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<CursoDTO>>(cursos);

        }



        //GET api/curso/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CursoPreviewDTO>> GetById(int id)
        {
            var curso = await context.Cursos.FirstOrDefaultAsync(x => x.Id == id);
            if (curso is null)
            {
                return BadRequest($"El curso {id} no existe.");
            }

            var cursoPreviewDTO = mapper.Map<CursoPreviewDTO>(curso);

            //consulto los archivos adjuntos
            var archivos = await context.ArchivosCursos
                .Where(x => x.IdCurso == id)
                .ToListAsync();

            cursoPreviewDTO.ArchivosCurso = mapper.Map<List<ArchivoCursoDTO>>(archivos);

            return cursoPreviewDTO;

        }

        //GET api/curso/obtenerArchivosCurso/{idCurso:int}
        [HttpGet("obtenerArchivosCurso/{idCurso:int}")]
        public async Task<ActionResult<List<ArchivoCursoDTO>>> ObtenerArchivosCurso(int idCurso)
        {
            //consulto los archivos adjuntos
            var archivos = await context.ArchivosCursos
                .Where(x => x.IdCurso == idCurso)
                .ToListAsync();

            return mapper.Map<List<ArchivoCursoDTO>>(archivos);
        }


        //POST api/cursos/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromForm] CursoCreacionDTO cursoCreacionDTO)
        {

            int idUsuario = userSession.GetUserSessionId();

            if (idUsuario == 0)
            {
                return BadRequest("Acceso denegado, no estás autenticado.");
            }

            cursoCreacionDTO.TrimTextProperties();

            var curso = mapper.Map<Curso>(cursoCreacionDTO);

            if (cursoCreacionDTO.Fondo != null)
            {
                curso.FondoURL = await almacenadorArchivos.GuardarArchivoAsync(contenedor, cursoCreacionDTO.Fondo);
            }

            //Estados
            /*
             Pendiente = 0
             Publico = 1             
             */

            curso.IdCreadoPor = idUsuario;
            curso.Estado = 0;
            curso.FechaCreacion = DateTime.Now;
            curso.UltimaModificacion = DateTime.Now;

            await context.AddAsync(curso);
            await context.SaveChangesAsync();

            //Agregar la lista de los archivos adjuntos
            if (cursoCreacionDTO.ArchivosAdjuntos != null)
            {
                foreach (var item in cursoCreacionDTO.ArchivosAdjuntos)
                {
                    var archivoCurso = new ArchivosCurso
                    {
                        IdCurso = curso.Id,
                        ArchivoNombre = item.FileName,
                        ArchivoURL = await almacenadorArchivos.GuardarArchivoAsync(contenedorArchivos, item)
                    };

                    await context.AddAsync(archivoCurso);
                }

                await context.SaveChangesAsync();
            }

            return NoContent();
        }


        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromForm] CursoCreacionDTO cursoEditarDTO)
        {

            int idUsuario = userSession.GetUserSessionId();

            if (idUsuario == 0)
            {
                return BadRequest("Acceso denegado, no estás autenticado.");
            }

            var curso = await context.Cursos.FirstOrDefaultAsync(x => x.Id == id);
            if (curso == null)
            {
                return BadRequest($"El curso {id} no existe.");
            }

            cursoEditarDTO.TrimTextProperties();

            curso = mapper.Map(cursoEditarDTO, curso);

            //Si existe archivo lo elimino y pongo nuevo
            if (cursoEditarDTO.Fondo != null)
            {
                curso.FondoURL = await almacenadorArchivos.EditarArchivoAsync(contenedor, cursoEditarDTO.Fondo, curso.FondoURL);
            }

            curso.UltimaModificacion = DateTime.Now;

            await context.SaveChangesAsync();

            //Agregar la lista de los archivos adjuntos
            if (cursoEditarDTO.ArchivosAdjuntos != null)
            {
                foreach (var item in cursoEditarDTO.ArchivosAdjuntos)
                {
                    var archivoCurso = new ArchivosCurso
                    {
                        IdCurso = curso.Id,
                        ArchivoNombre = item.FileName,
                        ArchivoURL = await almacenadorArchivos.GuardarArchivoAsync(contenedorArchivos, item)
                    };

                    await context.AddAsync(archivoCurso);
                }

                await context.SaveChangesAsync();
            }

            return NoContent();

        }

        [HttpPut("publicar/{id:int}")]
        public async Task<ActionResult> Publicar(int id)
        {
            //Estados
            /*
             Pendiente = 0
             Publico = 1             
             */

            var curso = await context.Cursos.FirstOrDefaultAsync(x => x.Id == id);
            if (curso == null)
            {
                return BadRequest($"El curso {id} no existe.");
            }

            curso.Estado = 1;
            curso.UltimaModificacion = DateTime.Now;

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("eliminarCurso/{id:int}")]
        public async Task<ActionResult> EliminarCurso(int id)
        {
            var curso = await context.Cursos.FirstOrDefaultAsync(x => x.Id == id);
            if (curso == null)
            {
                return BadRequest($"El curso {id} no existe.");
            }

            //Eliminar los archivos adjuntos del curso, si es que existen.
            var archivosAdjuntos = await context.ArchivosCursos
                .Where(x => x.IdCurso == curso.Id)
                .ToListAsync();

            //Si existen archivos adjuntos
            if (archivosAdjuntos.Count > 0)
            {
                foreach (var item in archivosAdjuntos)
                {
                    //Removemos el archivo
                    await almacenadorArchivos.BorrarArchivoAsync(item.ArchivoURL, contenedorArchivos);
                    context.Remove(item);
                }
            }

            context.Remove(curso);

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("eliminarArchivo/{idArchivo:int}")]
        public async Task<ActionResult> EliminarArchivo(int idArchivo)
        {
            var archivo = await context.ArchivosCursos
                .FirstOrDefaultAsync(x => x.Id == idArchivo);

            if (archivo is null)
            {
                return BadRequest($"El archivo {idArchivo} no existe.");
            }

            context.Remove(archivo);
            await context.SaveChangesAsync();

            await almacenadorArchivos.BorrarArchivoAsync(archivo.ArchivoURL, contenedorArchivos);

            return NoContent();
        }




    }
}
