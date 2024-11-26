using System;
using System.Collections.Generic;

namespace AvaloniaApplication1.Models;

public partial class Gender
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Клиенты> Клиентыs { get; set; } = new List<Клиенты>();
}
