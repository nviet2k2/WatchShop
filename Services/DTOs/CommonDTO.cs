using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class CommonDTO
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedDT { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDT { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;
    }
}
