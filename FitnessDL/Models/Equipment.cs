using System;
using System.Collections.Generic;

namespace FitnessDL.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string DeviceType { get; set; } = null!;

    public bool IsInMaintenance { get; set; }

    public virtual ICollection<ReservationTimeSlot> ReservationTimeSlots { get; set; } = new List<ReservationTimeSlot>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
