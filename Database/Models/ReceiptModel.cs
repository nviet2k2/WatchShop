using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ReceiptModel : CommonModel
    {
        public int Id { get; set; }
        public DateTime CreateDate  { get; set; }
        public int? SupplierId { get; set; }
        public virtual SupplierModel? Supplier { get; set; }
        public int? UserId { get; set; }
        public virtual UserModel? User { get; set; }
        public double Total { get; set; }
        
    }
}
