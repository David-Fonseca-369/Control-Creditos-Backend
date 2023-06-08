using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.Entities
{
    public class Curso
    {
        public int Id { get; set; }
        [Required]
        public int IdCreadoPor { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public int LimiteEstudiantes { get; set; }
        [Required, MaxLength(60)]
        public string FondoURL { get; set; }
        [Required]
        public int Estado { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public DateTime UltimaModificacion { get; set; }

        //References
        public Usuario CreadoPor { get; set; }
    }
}
