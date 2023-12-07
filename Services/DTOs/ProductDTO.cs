using Database.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ProductDTO : CommonDTO
        {
            public int Id { get; set; }
            public string ProductName { get; set; }
            public string? Img { get; set; }
            public double Price { get; set; } = 0;
            public int Quantity { get; set; } = 0;
            public int? BrandId { get; set; }
            public string Size { get; set; }
            public string Thumnail { get; set; }
            public string Color { get; set; }
            public string Description { get; set; }
            public string Code { get; set; }
            public string Gender { get; set; }
            public string Status { get; set; }
            public int? CategoryId { get; set; }

        }
    public class CreateProductDTO : CommonDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int? BrandId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public int? CategoryId { get; set; }

    }
}
