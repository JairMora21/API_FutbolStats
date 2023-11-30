namespace API_FutbolStats.Models.Dto
{
    public class GolesDto
    {
        public int Id { get; set; }

        public int? IdJugador { get; set; }

        public int? IdPartido { get; set; }

        public int? IdTemporada { get; set; }

        public int? IdEquipo { get; set; }
    }
}
