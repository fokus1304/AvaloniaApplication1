using System;
using System.Collections.Generic;

namespace AvaloniaApplication1.Models;

public partial class FileList
{
    public int Id { get; set; }

    public string? File { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();
}
