using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoCreate
{
    public class TemporadaDtoCreate
    {

        [Required(ErrorMessage = "El campo Equipo es obligatorio.")]
        public int? IdEquipo { get; set; }

        [Required(ErrorMessage = "El campo Temporada es obligatorio.")]
        public int? NoTemporada { get; set; }

        [Required(ErrorMessage = "El campo Fecha Inicio es obligatorio.")]
        public DateOnly FechaInicio { get; set; }

        public string? NombreTemporada { get; set; }
    }
}
