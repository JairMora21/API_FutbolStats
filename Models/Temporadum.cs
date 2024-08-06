using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class Temporadum
{
    public int Id { get; set; }

    public int? IdClasificacion { get; set; }

    public int? IdEquipo { get; set; }

    public int NoTemporada { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFinal { get; set; }

    public string? Posicion { get; set; }

    public string? NombreTemporada { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Gole> Goles { get; set; } = new List<Gole>();

    public virtual ClasificacionTemporadum? IdClasificacionNavigation { get; set; }

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();

    public virtual ICollection<PartidosJugado> PartidosJugados { get; set; } = new List<PartidosJugado>();

    public virtual ICollection<Tarjetum> Tarjeta { get; set; } = new List<Tarjetum>();
}
