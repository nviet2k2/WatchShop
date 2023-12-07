using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class CreateAddressDTO
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int CustomerId { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string HouseNumber { get; set; }
        public string Note { get; set; }
        public bool status { get; set; }
        public string Province { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class AddressDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string HouseNumber { get; set; }
        public string Note { get; set; }
        public string Province { get; set; }
        public string PhoneNumber { get; set; }
        public bool status { get; set; }

    }

}
