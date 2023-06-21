using System;
using System.Collections.Generic;

namespace HomeTaskerAPI.Models;

public partial class Moradore
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Apartamento { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public bool? IsAdministrador { get; set; }

    public virtual ICollection<Conta> Conta { get; set; } = new List<Conta>();

    public virtual ICollection<Ocorrencia> Ocorrencia { get; set; } = new List<Ocorrencia>();

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<Servico> Servicos { get; set; } = new List<Servico>();

    public virtual ICollection<Visitante> Visitantes { get; set; } = new List<Visitante>();
}
