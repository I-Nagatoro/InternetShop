using System;
using System.Collections.Generic;

namespace InternetShop.Models;

public partial class BasketProduct
{
    public int Id { get; set; }

    public int BasketId { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }
}
