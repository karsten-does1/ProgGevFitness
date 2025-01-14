using System;
using System.Collections.Generic;

namespace FitnessDL.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int EquipmentId { get; set; }

    public int TimeSlotId { get; set; }

    public DateOnly Date { get; set; }

    public int MemberId { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<ReservationTimeSlot> ReservationTimeSlots { get; set; } = new List<ReservationTimeSlot>();

    public virtual TimeSlot TimeSlot { get; set; } = null!;
}
