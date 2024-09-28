using System;
using System.Collections.Generic;

namespace MyGoParking.Models;

public partial class Ewallet
{
    public int WalletId { get; set; }

    public int? UserId { get; set; }

    public int Balance { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public virtual Customer? User { get; set; }
}
