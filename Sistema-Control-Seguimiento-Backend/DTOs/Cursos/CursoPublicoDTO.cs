namespace Sistema_Control_Seguimiento_Backend.DTOs.Cursos
{
    public class CursoPublicoDTO
    {
        public int Id { get; set; }
        public int IdCreadoPor { get; set; }
        public string NombreInstructor  { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int LimiteEstudiantes { get; set; }
        public int Inscritos { get; set; }
        public string FondoURL { get; set; }
        public bool SolicitudEnviada { get; set; }
    }
}
