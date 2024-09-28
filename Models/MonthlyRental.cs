using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class MonthlyRental
{
    public int MonId { get; set; }

    public int? LotId { get; set; }

    public int? CarId { get; set; }

    public int? SlotId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool RenewalStatus { get; set; }

    public bool NotificationStatus { get; set; }

    public int? Amount { get; set; }

    public DateTime? PaymentTime { get; set; }

    public bool PaymentStatus { get; set; }

    public virtual Car? Car { get; set; }

    public virtual ParkingLot? Lot { get; set; }

    public virtual ParkingSlot? Slot { get; set; }
}
