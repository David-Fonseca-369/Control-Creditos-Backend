using Sistema_Control_Seguimiento_Backend.DTOs.ArchivosCursos;

namespace Sistema_Control_Seguimiento_Backend.DTOs.Cursos
{
    public class CursoPreviewDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int LimiteEstudiantes { get; set; }
        public string FondoURL { get; set; }

        public List<ArchivoCursoDTO> ArchivosCurso { get; set; }
    }
}
