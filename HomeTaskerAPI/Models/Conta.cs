using System;
using System.Collections.Generic;

namespace HomeTaskerAPI.Models;

public partial class Conta
{
    public int Id { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataVencimento { get; set; }

    public string Status { get; set; } = null!;

    public int? MoradorId { get; set; }

    public virtual Moradore? Morador { get; set; }
}
