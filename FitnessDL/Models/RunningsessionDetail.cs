using System;
using System.Collections.Generic;

namespace FitnessDL.Models;

public partial class RunningsessionDetail
{
    public int RunningsessionId { get; set; }

    public int SeqNr { get; set; }

    public int IntervalTime { get; set; }

    public double IntervalSpeed { get; set; }

    public virtual RunningsessionMain Runningsession { get; set; } = null!;
}
