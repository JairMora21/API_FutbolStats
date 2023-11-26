using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class Jugador
{
    public int Id { get; set; }

    public int IdPosicion { get; set; }

    public int IdEquipo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Img { get; set; }

    public string? Dorsal { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Gole> Goles { get; set; } = new List<Gole>();

    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    public virtual PosicionJugador IdPosicionNavigation { get; set; } = null!;

    public virtual ICollection<PartidosJugado> PartidosJugados { get; set; } = new List<PartidosJugado>();

    public virtual ICollection<Tarjetum> Tarjeta { get; set; } = new List<Tarjetum>();
}
