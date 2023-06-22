namespace Sistema_Control_Seguimiento_Backend.DTOs.Cursos
{
    public class CursoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
        public DateTime UltimaModificacion { get; set; }
    }
}
