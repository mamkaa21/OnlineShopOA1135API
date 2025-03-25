using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class Role
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
