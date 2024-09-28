using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class EntryExitManagement
{
    public int EntryexitId { get; set; }

    public int? LotId { get; set; }

    public int? CarId { get; set; }

    public string? Parktype { get; set; }

    public string? LicensePlatePhoto { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? LicensePlateKeyinTime { get; set; }

    public int? Amount { get; set; }

    public DateTime? PaymentTime { get; set; }

    public bool PaymentStatus { get; set; }

    public DateTime? ExitTime { get; set; }

    public DateTime? ValidTime { get; set; }

    public bool IsFinish { get; set; }

    public virtual Car? Car { get; set; }

    public virtual ParkingLot? Lot { get; set; }
}
