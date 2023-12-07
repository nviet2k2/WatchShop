using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class CreateReceiptDTO
    {

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int? SupplierId { get; set; }
        //public SupplierDTO Supplier { get; set; } 
        [JsonIgnore]
        public int? UserId { get; set; }
        //public UserModel User { get; set; } 
        
        public List<CreateReceiptDetailDTO> ReceiptDetails { get; set; }
    }
    public class CreateReceiptDetailDTO
    {
        public int Id { get; set; }
        public int? ReceiptId { get; set; }  
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
