using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace InternetShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Cost { get; set; }

    public string? ProductCaption { get; set; }

    public string? ProductImage { get; set; }

    public Bitmap? ParseImage 
    {
        get
        {
            return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + ProductImage);
        }
    }
}
