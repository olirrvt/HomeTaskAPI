using System;
using System.Collections.Generic;

namespace HomeTaskerAPI.Models;

public partial class Aviso
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Mensagem { get; set; } = null!;

    public DateTime DataPublicacao { get; set; }
}
