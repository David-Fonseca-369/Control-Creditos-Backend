using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.Entities
{
    public class ArchivosCurso
    {
        public int Id { get; set; }
        [Required]
        public int IdCurso { get; set; }
        [Required, MaxLength(60)]
        public string ArchivoURL { get; set; }
        [Required, MaxLength(60)]
        public string ArchivoNombre { get; set; }

        //references
        public Curso Curso { get; set; }
    }
}
