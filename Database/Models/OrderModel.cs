using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class OrderModel : CommonModel
    {
        public int Id { get; set; }
        public string DeliverAddress { get; set; }
        public DateTime CreateDate { get; set; }
        public double TotalPrice { get; set; }
        public int? UserId { get; set; }
        public virtual UserModel? User { get; set; }
        public int? VoucherId { get;set;}
        public virtual VoucherModel? Voucher { get; set; }
        public string? OrderStatus { get; set; }
        public string Note { get; set; }
        public string OrderCode { get; set; }
    }
}
