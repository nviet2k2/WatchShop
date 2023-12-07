using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class RolePermissionModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public virtual RoleModel Role { get; set; }
        public int PermissionId { get; set; }
        public virtual PermissionModel Permission { get; set; }
    }
}
