using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class Good
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public string? Review { get; set; }

    public int? Rating { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderGoodsCross> OrderGoodsCrosses { get; set; } = new List<OrderGoodsCross>();
}
public class GoodModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public string? Review { get; set; }

    public int? Rating { get; set; }
}