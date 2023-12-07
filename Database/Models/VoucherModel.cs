using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class VoucherModel : CommonModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
