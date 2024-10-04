using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class ParkingLot
{
    public int LotId { get; set; }

    public string? LotName { get; set; }

    public string? LotAddress { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public int? Qty { get; set; }

    public int? Etcqty { get; set; }

    public int? Monqty { get; set; }

    public bool IsResStatus { get; set; }

    public bool IsMonStatus { get; set; }

    public string? Contract { get; set; }

    public virtual ICollection<EntryExitManagement> EntryExitManagement { get; set; } = new List<EntryExitManagement>();

    public virtual ICollection<MonApplyList> MonApplyList { get; set; } = new List<MonApplyList>();

    public virtual ICollection<MonthlyRental> MonthlyRental { get; set; } = new List<MonthlyRental>();

    public virtual ICollection<ParkingSlot> ParkingSlot { get; set; } = new List<ParkingSlot>();

    public virtual ICollection<Reservation> Reservation { get; set; } = new List<Reservation>();
}
