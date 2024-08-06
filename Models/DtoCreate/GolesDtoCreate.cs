using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoCreate
{
    public class GolesDtoCreate
    {

        [Required(ErrorMessage = "El campo IdJugador es obligatorio.")]
        public int? IdJugador { get; set; }

        [Required(ErrorMessage = "El campo IdPartido es obligatorio.")]
        public int? IdPartido { get; set; }

        [Required(ErrorMessage = "El campo IdTemporada es obligatorio.")]
        public int? IdTemporada { get; set; }

        [Required(ErrorMessage = "El campo IdEquipo es obligatorio.")]
        public int? IdEquipo { get; set; }

        [Required(ErrorMessage = "El campo Goles es obligatorio.")]
        public int? Goles { get; set; }
    }
}
