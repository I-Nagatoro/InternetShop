using System;
using System.Collections.Generic;

namespace InternetShop.Models;

public partial class OrderProduct
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductsId { get; set; }

    public int Count { get; set; }
}
