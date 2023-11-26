using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class TipoPartido
{
    public int Id { get; set; }

    public string? TipoPartido1 { get; set; }

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();
}
