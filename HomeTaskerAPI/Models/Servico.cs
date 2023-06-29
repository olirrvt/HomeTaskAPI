using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual Moradore? Morador { get; set; }
    }
}
