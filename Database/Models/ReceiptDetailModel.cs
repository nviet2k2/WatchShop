using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ReceiptDetailModel : CommonModel
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public virtual ReceiptModel? Receipt { get; set; }
        public int? ProductId { get; set; }
        public virtual ProductModel? Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
