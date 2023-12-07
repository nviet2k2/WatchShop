using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class OrganizationPersonaTitleDTO
    {
        public int Id { get; set; }
        public required string TitleName { get; set; }
        public string TitleCode { get; set; }
        public string? Note { get; set; }
    }
}
