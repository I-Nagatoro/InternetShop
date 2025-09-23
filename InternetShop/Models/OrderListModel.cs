using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Models
{
    public partial class OrderListModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int OrderId { get; set; }
        public decimal SumCost { get; set; }
    }
}
