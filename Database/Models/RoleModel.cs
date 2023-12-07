using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class RoleModel:CommonModel
    {
        public int Id { get; set; }
        public string RoleTitle { get; set; }
        public string Description { get; set; }
    }
}
