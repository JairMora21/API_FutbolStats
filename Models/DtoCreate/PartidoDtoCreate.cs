using System.ComponentModel.DataAnnotations;

namespace API_FutbolStats.Models.DtoCreate
{
    public class PartidoDtoCreate
    {

        public Result Result { get; set; }
        public List<PlayerStat> PlayerStats { get; set; }

    }


    public class Result
    {
        public string NombreRival { get; set; }
        public int GolesFavor { get; set; }
        public int GolesContra { get; set; }
        public DateOnly Fecha { get; set; }
        public int IdTemporada { get; set; }
        public int IdResultado { get; set; }
        public int IdTipoPartido { get; set; }
        public int IdEquipo { get; set; }
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
