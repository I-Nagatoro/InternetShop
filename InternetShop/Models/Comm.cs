using System;
using System.Collections.Generic;

namespace InternetShop.Models;

public partial class Comm
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string Comm1 { get; set; } = null!;
}
