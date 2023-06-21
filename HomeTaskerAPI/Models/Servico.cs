using System;
using System.Collections.Generic;

namespace HomeTaskerAPI.Models
{
    public partial class Servico
    {
        public int Id { get; set; }

        public string TipoServico { get; set; } = null!;

        public string Descricao { get; set; } = null!;

        public DateTime DataHoraSolicitacao { get; set; }

        public string StatusDoServico { get; set; } = null!;

        public int? MoradorId { get; set; }

        public virtual Moradore? Morador { get; set; }
    }
}
