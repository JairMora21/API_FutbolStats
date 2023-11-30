using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoUpdate
{
    public class GolesDtoUpdate
    {
        [Required(ErrorMessage = "El campo IdJugador es obligatorio.")]
        public int? IdJugador { get; set; }

        [Required(ErrorMessage = "El campo IdPartido es obligatorio.")]
        public int? IdPartido { get; set; }

        [Required(ErrorMessage = "El campo IdTemporada es obligatorio.")]
        public int? IdTemporada { get; set; }

        [Required(ErrorMessage = "El campo IdEquipo es obligatorio.")]
        public int? IdEquipo { get; set; }
    }
}
