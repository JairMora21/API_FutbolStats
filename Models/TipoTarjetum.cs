using System;
using System.Collections.Generic;

namespace API_FutbolStats.Models;

public partial class TipoTarjetum
{
    public int Id { get; set; }

    public string? Tarjeta { get; set; }

    public virtual ICollection<Tarjetum> TarjetaNavigation { get; set; } = new List<Tarjetum>();
}
