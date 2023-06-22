using Microsoft.AspNetCore.Mvc;
using Sistema_Control_Seguimiento_Backend.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Seguimiento_Backend.DTOs.Cursos
{
    public class CursoCreacionDTO
    {
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
        public IFormFile Fondo { get; set; }

        //[ModelBinder(BinderType = typeof(TypeBinder<List<IFormFile>>))]
        public List<IFormFile> ArchivosAdjuntos { get; set; }
    }
}
