using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class UserModel : CommonModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string? PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public bool IsLockedout { get; set; } = false;
        public DateTime? LastOnline { get; set; }
        public int? EmployeeId { get; set; }
        public virtual EmployeeModel Employee { get; set; }
        public int? CustomerId { get; set; }
        public virtual CustomerModel Customer { get; set; }
        public int RoleId { get; set; }
        public virtual RoleModel Role { get; set; }
        public List<RoleModel> Roles { get; set; }
        //public virtual List<int>? RoleIds { get; set; }
        //public virtual List<string> Permissions { get; set; }
    }
}
