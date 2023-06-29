using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.DTOs.Solicitudes
{
    public class SolicitudCreacionDTO
    {
        [Required]
        public int IdCurso { get; set; }
        [Required]
        public int IdAlumno { get; set; }
        [MaxLength(250)]
        public string ComentariosSolicitud { get; set; }
    }
}
