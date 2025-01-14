using System;
using System.Collections.Generic;

namespace FitnessDL.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string Address { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public string? Interests { get; set; }

    public string? Membertype { get; set; }

    public virtual ICollection<Cyclingsession> Cyclingsessions { get; set; } = new List<Cyclingsession>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<RunningsessionMain> RunningsessionMains { get; set; } = new List<RunningsessionMain>();

    public virtual ICollection<Program> ProgramCodes { get; set; } = new List<Program>();
}
