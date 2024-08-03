using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoCreate
{
    public class PartidoDtoCreate
    {
        //[Required(ErrorMessage = "El campo Equipo es obligatorio.")]
        //public int IdEquipo { get; set; }

        //[Required(ErrorMessage = "El campo Temporada es obligatorio.")]
        //public int? IdTemporada { get; set; }

        //[Required(ErrorMessage = "El campo TipoPartido es obligatorio.")]
        //public int IdTipoPartido { get; set; }

        //[Required(ErrorMessage = "El campo Resultado es obligatorio.")]
        //public int IdResultado { get; set; }

        //[Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        //public DateOnly Fecha { get; set; }

        //[Required(ErrorMessage = "El campo Nombre Rival es obligatorio.")]
        //public string NombreRival { get; set; } = null!;

        //[Required(ErrorMessage = "El campo Goles Favor es obligatorio.")]
        //public int GolesFavor { get; set; }

        //[Required(ErrorMessage = "El campo Goles Contra es obligatorio.")]
        //public int GolesContra { get; set; }
        public Result Result { get; set; }
        public List<PlayerStat> PlayerStats { get; set; }

    }


    public class Result
    {
        public string NombreRival { get; set; }
        public int GolesFavor { get; set; }
        public int GolesContra { get; set; }
        public DateTime Fecha { get; set; }
        public int TypeOfMatch { get; set; }
        public int seasonId { get; set; }
    }

    public class PlayerStat
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Dorsal { get; set; }
        public int Goles { get; set; }
        public int Amarillas { get; set; }
        public int Rojas { get; set; }
    }


}
