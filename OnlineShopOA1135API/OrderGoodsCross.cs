using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class OrderGoodsCross
{
    public int? OrderId { get; set; }

    public int? GoodsId { get; set; }

    public virtual Good? Goods { get; set; }

    public virtual Order? Order { get; set; }
}
