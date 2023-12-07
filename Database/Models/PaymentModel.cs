using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public string transactionId { get; set; }
        public string status { get; set; }
        public string PaymentCode { get; set; }
    }
}
