namespace API_FutbolStats.Models.DtoStats
{
    public class PartidoDtoStats
    {

        public int Id { get; set; }

        public string Equipo { get; set; }

        public string Temporada { get; set; }

        public string TipoPartido { get; set; }

        public string Resultado { get; set; }

        public DateOnly Fecha { get; set; }

        public string NombreRival { get; set; } = null!;

        public int GolesFavor { get; set; }

        public int GolesContra { get; set; }
    }
}
