using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class ParkingSlot
{
    public int SlotId { get; set; }

    public int? LotId { get; set; }

    public string? SlotNumber { get; set; }

    public bool IsRented { get; set; }

    public virtual ParkingLot? Lot { get; set; }

    public virtual ICollection<MonthlyRental> MonthlyRental { get; set; } = new List<MonthlyRental>();
}
