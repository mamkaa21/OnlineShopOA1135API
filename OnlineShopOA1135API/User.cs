﻿using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class User
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
