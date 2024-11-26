using System;
using System.Collections.Generic;

namespace AvaloniaApplication1.Models;

public partial class File
{
    public int Id { get; set; }

    public int? IdKlient { get; set; }

    public int? IdSpisok { get; set; }

    public virtual Клиенты? IdKlientNavigation { get; set; }

    public virtual FileList? IdSpisokNavigation { get; set; }

    public virtual Клиенты? Клиенты { get; set; }
}
