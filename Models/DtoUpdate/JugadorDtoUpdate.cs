using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoUpdate
{
    public class JugadorDtoUpdate
    {
        [Required]
        public int IdPosicion { get; set; }
        [Required]
        public int IdEquipo { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Apellido { get; set; } = null!;

        public string? Img { get; set; }

        public string? Dorsal { get; set; }

        public bool Activo { get; set; }
    }
}
