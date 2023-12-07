using Database.Models;
using Database;
using Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
   
    public interface IEmployeeRepository : IBaseRepository<EmployeeModel>
    {

    }
    public class EmployeeRepository : BaseRepository<EmployeeModel>, IEmployeeRepository
    {
        public EmployeeRepository(WebApiContext context) : base(context)
        {

        }
    }
}
