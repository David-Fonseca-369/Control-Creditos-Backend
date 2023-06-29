namespace Sistema_Control_Seguimiento_Backend.DTOs.Solicitudes
{
    public class SolicitudDTO
    {
        public int Id { get; set; }
        public string NombreCurso { get; set; }
        public string NombreInstructor { get; set; }
        public int Estado { get; set; }
        public string ConstanciaURL { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
