using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeTaskerAPI.Models;

public partial class Produto
{
    public int Id { get; set; }

    public string NomeProduto { get; set; } = null!;

    public string? DescricaoProduto { get; set; }

    public decimal Preco { get; set; }

    public int? MoradorId { get; set; }

    [JsonIgnore]
    public virtual Moradore? Morador { get; set; }
}
