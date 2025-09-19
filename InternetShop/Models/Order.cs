using System;
using System.Collections.Generic;

namespace InternetShop.Models;

public partial class Order
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal SumCost { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string CodePickup { get; set; } = null!;
}
