namespace API_FutbolStats.Models.DtoCreate
{
    public class JugadorDtoCreate
    {

        public int IdPosicion { get; set; }

        public int IdEquipo { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Img { get; set; }

        public string? Dorsal { get; set; }

        public bool Activo { get; set; }
    }
}
