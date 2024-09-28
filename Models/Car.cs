using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class Car
{
    public int CarId { get; set; }

    public int? UserId { get; set; }

    public string? LicensePlate { get; set; }

    public virtual ICollection<EntryExitManagement> EntryExitManagement { get; set; } = new List<EntryExitManagement>();

    public virtual ICollection<MonApplyList> MonApplyList { get; set; } = new List<MonApplyList>();

    public virtual ICollection<MonthlyRental> MonthlyRental { get; set; } = new List<MonthlyRental>();

    public virtual ICollection<Reservation> Reservation { get; set; } = new List<Reservation>();

    public virtual Customer? User { get; set; }
}
