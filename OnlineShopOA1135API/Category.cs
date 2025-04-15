using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnlineShopOA1135API;

public partial class Category
{
    public int Id { get; set; }

    public string? Title { get; set; }

    [JsonIgnore]
    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
