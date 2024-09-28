using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class MonApplyList
{
    public int ApplyId { get; set; }

    public int? LotId { get; set; }

    public int? CarId { get; set; }

    public string? ApplyStatus { get; set; }

    public DateTime? ApplyDate { get; set; }

    public virtual Car? Car { get; set; }

    public virtual ParkingLot? Lot { get; set; }
}
