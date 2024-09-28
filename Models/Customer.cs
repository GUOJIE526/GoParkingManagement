using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class Customer
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int BlackCount { get; set; }

    public bool IsBlack { get; set; }

    public virtual ICollection<Car> Car { get; set; } = new List<Car>();

    public virtual ICollection<Ewallet> Ewallet { get; set; } = new List<Ewallet>();
}
