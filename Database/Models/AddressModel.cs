using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class AddressModel : CommonModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string HouseNumber { get; set; }
        public string Note { get; set; }
        public string? Province { get; set; }
        public string? PhoneNumber { get; set; }
        public bool status { get; set; }

    }
}
