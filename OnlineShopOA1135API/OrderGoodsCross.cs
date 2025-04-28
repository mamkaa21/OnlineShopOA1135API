using System;
using System.Collections.Generic;

namespace OnlineShopOA1135API;

public partial class OrderGoodsCross
{
    public int? OrderId { get; set; }

    public int? GoodsId { get; set; }

    public virtual Good? Goods { get; set; }

    public virtual Order? Order { get; set; } 
    //public int Quantity { get; set; } надо в бд добавить
}
// DTO (Data Transfer Object) для добавления товара в корзину.
public class AddToCartRequest
{
    public int GoodId { get; set; }
    // Добавлено количество товара
}
