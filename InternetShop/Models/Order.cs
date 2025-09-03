using System;
using System.Collections.Generic;

namespace InternetShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal SumCost { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
