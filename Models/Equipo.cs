using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class Equipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Lugar { get; set; }

    public string? Escudo { get; set; }

    public int IdUsuario { get; set; }

    public virtual ICollection<Gole> Goles { get; set; } = new List<Gole>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();

    public virtual ICollection<PartidosJugado> PartidosJugados { get; set; } = new List<PartidosJugado>();

    public virtual ICollection<Tarjetum> Tarjeta { get; set; } = new List<Tarjetum>();

    public virtual ICollection<Temporadum> Temporada { get; set; } = new List<Temporadum>();
}
