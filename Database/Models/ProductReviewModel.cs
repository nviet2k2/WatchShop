using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ProductReviewModel : CommonModel
    {
        [Key]
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
        public double Rating { get; set; }//1,,2,2.5,3.. sao
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
