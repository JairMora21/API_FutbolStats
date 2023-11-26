using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class ResultadoPartido
{
    public int Id { get; set; }

    public string? Resultado { get; set; }

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();
}
