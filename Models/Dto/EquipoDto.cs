namespace API_FutbolStats.Models.Dto
{
    public class EquipoDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Lugar { get; set; }

        public string? Escudo { get; set; }

    }
}
