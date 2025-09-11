using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Models
{
    public class Product_basket
    {
        public int ProductId { get; set; }
        public string? ProductName{ get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string? ImagePath { get; set; }
        public string Caption { get; set; }
        public Bitmap? ParseImage
        {
            get
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + ImagePath);
            }
        }
    }
}
