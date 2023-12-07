using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class VoucherDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
