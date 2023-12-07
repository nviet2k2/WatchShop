using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    //Table NhanSu
    public class EmployeeModel : CommonModel 
    {
        public int Id { get; set; }
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
            set
            {

            }
        }
        public string Code { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Gender { get; set; } //1: Male, 2: Female, 3: Others
        public DateTime DateOfBirth { get; set; }
       
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public int? OrganizationPersonaTitleId { get; set; }    //Chức danh Id
        public virtual OrganizationPersonaTitleModel? OrganizationPersonaTitle { get; set; } //Chức danh
       
    }
}
