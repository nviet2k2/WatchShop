using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class OrderPaymentModel
    {
        public int Id { get; set; }
        public int? PaymentId { get; set; }
        public virtual PaymentModel? Payment { get; set; }
        public int? OrderId { get; set; }
        public virtual OrderModel? Order { get; set; }

    }
}
