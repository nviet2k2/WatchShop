using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReceiptDTO
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int? SupplierId { get; set; }
        public SupplierModel Supplier { get; set; }
        public int? UserId { get; set; }
        public UserModel User { get; set; }
        public double Total { get; set; }
        public List<ReceiptDetailDTO> ReceiptDetails { get; set; }
    }
    public class ReceiptDetailDTO
    {
        public int Id { get; set; }
        public int? ReceiptId { get; set; }
        public ReceiptDTO Receipt { get; set; }
        public int? ProductId { get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
