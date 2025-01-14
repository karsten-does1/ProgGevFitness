using System;
using System.Collections.Generic;

namespace FitnessDL.Models;

public partial class Cyclingsession
{
    public int CyclingsessionId { get; set; }

    public DateTime Date { get; set; }

    public int Duration { get; set; }

    public int AvgWatt { get; set; }

    public int MaxWatt { get; set; }

    public int AvgCadence { get; set; }

    public int MaxCadence { get; set; }

    public string Trainingtype { get; set; } = null!;

    public string? Comment { get; set; }

    public int MemberId { get; set; }

    public virtual Member Member { get; set; } = null!;
}
