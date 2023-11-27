namespace API_FutbolStats.Models.Dto
{
    public class JugadorDto
    {
        public int Id { get; set; }

        public int IdPosicion { get; set; }

        public int IdEquipo { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Img { get; set; }

        public string? Dorsal { get; set; }

        public bool Activo { get; set; }

    }
}
