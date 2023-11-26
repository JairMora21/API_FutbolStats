using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class PosicionJugador
{
    public int Id { get; set; }

    public int? Tipo { get; set; }

    public string? Posicion { get; set; }

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();
}
