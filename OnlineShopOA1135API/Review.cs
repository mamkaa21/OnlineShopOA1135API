using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int GoodId { get; set; }

    public string? Text { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
