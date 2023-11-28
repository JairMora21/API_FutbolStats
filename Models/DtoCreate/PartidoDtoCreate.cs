using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoCreate
{
    public class PartidoDtoCreate
    {
        [Required(ErrorMessage = "El campo Equipo es obligatorio.")]
        public int? IdEquipo { get; set; }

        [Required(ErrorMessage = "El campo Temporada es obligatorio.")]
        public int? IdTemporada { get; set; }

        [Required(ErrorMessage = "El campo TipoPartido es obligatorio.")]
        public int IdTipoPartido { get; set; }

        [Required(ErrorMessage = "El campo Resultado es obligatorio.")]
        public int IdResultado { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "El campo Nombre Rival es obligatorio.")]
        public string NombreRival { get; set; } = null!;

        [Required(ErrorMessage = "El campo Goles Favor es obligatorio.")]
        public int GolesFavor { get; set; }

        [Required(ErrorMessage = "El campo Goles Contra es obligatorio.")]
        public int GolesContra { get; set; }
    }
}
