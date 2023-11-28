namespace API_FutbolStats.Models.Dto
{
    public class PartidoDto
    {
        public int Id { get; set; }

        public int? IdEquipo { get; set; }

        public int? IdTemporada { get; set; }

        public int IdTipoPartido { get; set; }

        public int IdResultado { get; set; }

        public DateOnly Fecha { get; set; }

        public string NombreRival { get; set; } = null!;

        public int GolesFavor { get; set; }

        public int GolesContra { get; set; }
    }
}
