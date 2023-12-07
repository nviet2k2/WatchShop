using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ProductReviewDTO
    {
        public int ReviewId { get; set; }
              public int UserId { get; set; }
        public int ProductId { get; set; }
        public double Rating { get; set; }//1,,2,2.5,3.. sao
        public string Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
    public class CreateProductReviewDTO
    {
        public int ReviewId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double Rating { get; set; }//1,,2,2.5,3.. sao
        public string Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
