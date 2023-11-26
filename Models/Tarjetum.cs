using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class Tarjetum
{
    public int Id { get; set; }

    public int IdTipoTarjeta { get; set; }

    public int? IdJugador { get; set; }

    public int? IdPartido { get; set; }

    public int? IdTemporada { get; set; }

    public int? IdEquipo { get; set; }

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual Jugador? IdJugadorNavigation { get; set; }

    public virtual Partido? IdPartidoNavigation { get; set; }

    public virtual Temporadum? IdTemporadaNavigation { get; set; }

    public virtual TipoTarjetum IdTipoTarjetaNavigation { get; set; } = null!;
}
