namespace Sistema_Control_Seguimiento_Backend.DTOs.Solicitudes
{
    public class SolicitudInstructorDTO
    {
        public int Id { get; set; }
        public string NombreCurso { get; set; }
        public string NombreAlumno { get; set; }
        public string CorreoAlumno { get; set; }
        public string NoCuentaAlumno { get; set; }
        public int Estado { get; set; }
        
        public string  ComentariosSolicitud { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
