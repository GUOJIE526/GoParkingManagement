using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class Reservation
{
    public int ResId { get; set; }

    public int? LotId { get; set; }

    public int? CarId { get; set; }

    public DateTime? ReservationTime { get; set; }

    public DateTime? ValidUntil { get; set; }

    public bool? DepositStatus { get; set; }

    public bool? IsOverdue { get; set; }

    public bool? IsCanceled { get; set; }

    public bool? NotificationStatus { get; set; }

    public int? Amount { get; set; }

    public bool? IsRefoundDeposit { get; set; }

    public bool? IsFinish { get; set; }

    public virtual Car? Car { get; set; }

    public virtual ParkingLot? Lot { get; set; }
}
