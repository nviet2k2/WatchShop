using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ProductModel : CommonModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string? Img { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int? BrandId { get; set; }
        public string Size { get; set; }
        public string Thumnail { get; set; } 
        public string Color { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public virtual BrandModel? Brand { get; set; }
        public int? CategoryId { get; set; }
        public virtual CategoryModel? Category { get; set; }
        //public int? AccessoryId { get; set; }
        //public virtual AccessoryModel? Accessory { get; set; }



    }
}
