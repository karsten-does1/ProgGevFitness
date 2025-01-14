using System;
using System.Collections.Generic;

namespace FitnessDL.Models;

public partial class Program
{
    public string ProgramCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Target { get; set; } = null!;

    public DateTime Startdate { get; set; }

    public int MaxMembers { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
