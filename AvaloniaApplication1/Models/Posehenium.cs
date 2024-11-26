using System;
using System.Collections.Generic;

namespace AvaloniaApplication1.Models;

public partial class Posehenium
{
    public int Id { get; set; }

    public int? IdKlient { get; set; }

    public int? IdPosh { get; set; }

    public virtual Клиенты? IdKlientNavigation { get; set; }

    public virtual VisitTabl? IdPoshNavigation { get; set; }

    public virtual Клиенты? Клиенты { get; set; }
}
