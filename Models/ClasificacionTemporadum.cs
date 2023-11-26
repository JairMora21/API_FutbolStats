using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class ClasificacionTemporadum
{
    public int Id { get; set; }

    public string? Clasificacion { get; set; }

    public virtual ICollection<Temporadum> Temporada { get; set; } = new List<Temporadum>();
}
