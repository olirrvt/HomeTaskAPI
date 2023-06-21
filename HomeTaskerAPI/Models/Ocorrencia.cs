using System;
using System.Collections.Generic;

namespace HomeTaskerAPI.Models;

public partial class Ocorrencia
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime DataHoraRegistro { get; set; }

    public int? MoradorId { get; set; }

    public virtual Moradore? Morador { get; set; }
}
