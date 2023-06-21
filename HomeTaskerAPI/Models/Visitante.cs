using System;
using System.Collections.Generic;

namespace HomeTaskerAPI.Models
{
    public partial class Visitante
    {
        public int Id { get; set; }

        public string? NomeVisitante { get; set; }

        public DateTime DataHoraEntrada { get; set; }

        public DateTime DataHoraSaida { get; set; }

        public int? MoradorId { get; set; }

        public virtual Moradore? Morador { get; set; }
    }
}