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
}
