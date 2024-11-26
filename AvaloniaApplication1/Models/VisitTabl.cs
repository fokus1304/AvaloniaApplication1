using System;
using System.Collections.Generic;

namespace AvaloniaApplication1.Models;

public partial class VisitTabl
{
    public int Id { get; set; }

    public DateOnly? Data { get; set; }

    public TimeOnly? Time { get; set; }

    public virtual ICollection<Posehenium> Posehenia { get; set; } = new List<Posehenium>();
}
