using System;
using System.Collections.Generic;
using System.Text;

namespace StockTrading.Libraries.Models
{
    public class StonkOrder
    {
        public List<string> Symbols { get; set; }
        public int Quantity { get; set; }
        public decimal TrailAmount { get; set; }
    }
}
