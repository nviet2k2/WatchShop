using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string DeliverAddress { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public double TotalPrice { get; set; }
        public int? UserId { get; set; }
        public int? VoucherId { get; set; }
        public string? OrderStatus { get; set; }
        public string Note { get; set; }
       

    }
    public class CreateOrderDTO
    {
        public int Id { get; set; }
        public string DeliverAddress { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public double TotalPrice { get; set; }
        public int? VoucherId { get; set; }
        public string? OrderStatus { get; set; }
        public string Note { get; set; }
        public  List<OrderDetailDTO> details { get; set; }

    }
}
