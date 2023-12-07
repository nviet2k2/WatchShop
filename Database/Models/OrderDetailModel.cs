using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class OrderDetailModel : CommonModel
    {
        public int Id { get; set; }
        public int? BillId { get; set; }
        public virtual OrderModel? Bill { get; set; }
        public int? ProductId { get; set; }
        public virtual ProductModel? Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

    }
}
