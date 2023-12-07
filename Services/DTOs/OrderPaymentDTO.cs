using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class OrderPaymentDTO
    {
        public int Id { get; set; }
        public int? PaymentId { get; set; }
        public int? OrderId { get; set; }
       
    }
}
