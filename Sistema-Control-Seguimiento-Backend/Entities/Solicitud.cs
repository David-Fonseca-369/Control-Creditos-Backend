using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.Entities
{
    public class Solicitud
    {
        public int Id { get; set; }
        [Required]
        public int IdCurso { get; set; }
        [Required]
        public int IdAlumno { get; set; }
        [Required]
        public int Estado { get; set; }

        [MaxLength(255)]
        public string ConstanciaURL { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaRechazada { get; set; }
        public DateTime? FechaConstancia { get; set; }

        [MaxLength(255)]
        public string ComentariosSolicitud { get; set; }


        //References
        public Curso Curso { get; set; }
        public Alumno Alumno { get; set; }


    }
}
