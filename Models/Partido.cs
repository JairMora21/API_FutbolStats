using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class Partido
{
    public int Id { get; set; }

    public int? IdEquipo { get; set; }

    public int? IdTemporada { get; set; }

    public int IdTipoPartido { get; set; }

    public int IdResultado { get; set; }

    public DateOnly Fecha { get; set; }

    public string NombreRival { get; set; } = null!;

    public int GolesFavor { get; set; }

    public int GolesContra { get; set; }

    public virtual ICollection<Gole> Goles { get; set; } = new List<Gole>();

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual ResultadoPartido IdResultadoNavigation { get; set; } = null!;

    public virtual Temporadum? IdTemporadaNavigation { get; set; }

    public virtual TipoPartido IdTipoPartidoNavigation { get; set; } = null!;

    public virtual ICollection<PartidosJugado> PartidosJugados { get; set; } = new List<PartidosJugado>();

    public virtual ICollection<Tarjetum> Tarjeta { get; set; } = new List<Tarjetum>();
}
