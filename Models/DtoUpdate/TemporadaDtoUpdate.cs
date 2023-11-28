using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoUpdate
{
    public class TemporadaDtoUpdate
    {

        public int? IdClasificacion { get; set; }

        [Required(ErrorMessage = "El campo Equipo es obligatorio.")]
        public int? IdEquipo { get; set; }

        [Required(ErrorMessage = "El campo Equipo es obligatorio.")]
        public int NoTemporada { get; set; }

        [Required(ErrorMessage = "El campo Equipo es obligatorio.")]
        public DateOnly FechaInicio { get; set; }

        public DateOnly? FechaFinal { get; set; }

        public string? Posicion { get; set; }

        public string? NombreTemporada { get; set; }
    }
}
