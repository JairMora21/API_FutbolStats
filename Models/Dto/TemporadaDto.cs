namespace API_FutbolStats.Models.Dto
{
    public class TemporadaDto
    {
        public int Id { get; set; }

        public string Clasificacion { get; set; }

        public string Equipo { get; set; }

        public int NoTemporada { get; set; }

        public DateOnly FechaInicio { get; set; }

        public DateOnly? FechaFinal { get; set; }

        public string? Posicion { get; set; }

        public string? NombreTemporada { get; set; }
    }
}
