using System;
using System.Collections.Generic;

namespace FitnessDL.Models;

public partial class RunningsessionMain
{
    public int RunningsessionId { get; set; }

    public DateTime Date { get; set; }

    public int MemberId { get; set; }

    public int Duration { get; set; }

    public double AvgSpeed { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<RunningsessionDetail> RunningsessionDetails { get; set; } = new List<RunningsessionDetail>();
}
