using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
    public class ListOrderDetailDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderDTO? Order { get; set; } 
        public int? ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
