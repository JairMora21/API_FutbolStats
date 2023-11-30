namespace API_FutbolStats.Models.DtoUpdate
{
    public class TarjetaDtoUpdate
    {
        public int IdTipoTarjeta { get; set; }

        public int? IdJugador { get; set; }

        public int? IdPartido { get; set; }

        public int? IdTemporada { get; set; }

        public int? IdEquipo { get; set; }
    }
}
