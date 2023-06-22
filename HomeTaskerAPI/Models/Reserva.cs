using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeTaskerAPI.Models
{
    public partial class Reserva
    {
        public int Id { get; set; }

        public string EspacoComum { get; set; } = null!;

        public DateTime DataHoraReserva { get; set; }

        public int? MoradorId { get; set; }

        [JsonIgnore]
        public virtual Moradore? Morador { get; set; }
    }
}
