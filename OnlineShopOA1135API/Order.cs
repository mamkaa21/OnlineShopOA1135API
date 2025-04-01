using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CountProduct { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateStatusUpdated { get; set; }

    public virtual User? User { get; set; }
}
